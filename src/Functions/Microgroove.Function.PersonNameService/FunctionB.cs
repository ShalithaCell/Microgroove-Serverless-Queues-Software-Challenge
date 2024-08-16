using System;
using System.Net.Http;
using Azure;
using Azure.Storage.Queues.Models;
using Microgroove.Application.DTOs;
using Microgroove.Application.Services.ClientService;
using Microgroove.Application.Services.PersonService;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Microgroove.Function.PersonNameService
{
    public class FunctionB
    {
        private readonly ILogger<FunctionB> _logger;
        private readonly HttpClientService _httpClientService;
        private readonly IPersonService _personService;

        public FunctionB(ILogger<FunctionB> logger, IPersonService personService, HttpClientService httpClientService)
        {
            _logger = logger;
            _personService = personService;
            _httpClientService = httpClientService;
        }

        [Function(nameof(FunctionB))]
        public async Task Run([QueueTrigger("person-queue", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            // check if the messages is empty
            if (string.IsNullOrEmpty(message.MessageText))
            {
                _logger.LogInformation($"FunctionB: Triggered but no data in the queue message.");
                return;
            }

            _logger.LogInformation($"FunctionB: Processing queue item: {message.MessageText}");

            PersonDto? person;

            try
            {
                person = JsonConvert.DeserializeObject<PersonDto>(message.MessageText);

                if (person == null)
                {
                    throw new JsonException("Invalid queue message payload.");
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "FunctionB: Invalid queue message payload.");
                return; 
            }

            var fullName = $"{person.FirstName} {person.LastName}";

            var svgData = await _httpClientService.GetInitialsAsync(fullName);

            var status = await _personService.UpdatePersonSvgAsync(person, svgData);

            _logger.LogInformation(
                status
                    ? "FunctionB: Updated SVG data for {FirstName} {LastName}"
                    : "FunctionB: Cannot find record {FirstName} {LastName} for update the SVG data", person.FirstName,
                person.LastName);
        }
    }
}
