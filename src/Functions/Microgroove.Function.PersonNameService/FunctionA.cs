using Azure.Storage.Queues;
using FluentValidation.Results;
using Microgroove.Application.DTOs;
using Microgroove.Application.Services.PersonService;
using Microgroove.Application.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Microgroove.Function.PersonNameService
{
    public class FunctionA
    {
        private readonly ILogger<FunctionA> _logger;
        private readonly IPersonService _personService;
        private readonly QueueClient _queueClient;
        private readonly PersonDtoValidator _validator;

        public FunctionA(ILogger<FunctionA> logger, IPersonService personService, QueueClient queueClient, PersonDtoValidator validator)
        {
            _logger = logger;
            _personService = personService;
            _queueClient = queueClient;
            _validator = validator;
        }

        [Function("FunctionA")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed A request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            PersonDto? data;

            try
            {
                data = JsonConvert.DeserializeObject<PersonDto>(requestBody);

                if (data == null)
                {
                    throw new JsonException("RequestBody is Empty or Invalid JSON payload.");
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Invalid JSON payload.");
                return new BadRequestObjectResult("Invalid JSON payload.");
            }

            var validationResult = await _validator.ValidateAsync(data);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {errors}", validationResult.ToString());
                return new BadRequestObjectResult(validationResult.Errors);
            }

            var added = await _personService.AddPersonAsync(data);

            if (!added)
            {
                var message = "Person with the same FirstName and LastName already exists.";
                _logger.LogWarning(message);
                return new ConflictObjectResult(message);
            }

            // Enqueue message
            var messageContent = JsonConvert.SerializeObject(data);
            await _queueClient.CreateIfNotExistsAsync();
            await _queueClient.SendMessageAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(messageContent)));
            _logger.LogInformation("Enqueued message: {messageContent}", messageContent);

            return new OkObjectResult("Person added and message enqueued successfully.");
        }
    }
}
