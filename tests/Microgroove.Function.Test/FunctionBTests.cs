using Azure.Storage.Queues.Models;
using Microgroove.Application.DTOs;
using Microgroove.Application.Services.ClientService;
using Microgroove.Application.Services.PersonService;
using Microgroove.Function.PersonNameService;
using Microgroove.Function.Test.CustomLogger;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.Function.Test
{
    public class FunctionBTests
    {
        [Fact]
        public async Task RunLogsInformationWhenSvgUpdatedSuccessfully()
        {
            // Arrange
            var logger = new TestLogger<FunctionB>();

            var personServiceMock = new Mock<IPersonService>();
            var httpClientServiceMock = new Mock<IHttpClientService>();

            var validPersonDto = new PersonDto { FirstName = "John", LastName = "Doe" };
            var validJsonMessage = JsonConvert.SerializeObject(validPersonDto);

            QueueMessage queueMessage = QueuesModelFactory.QueueMessage(
                messageId: "",
                popReceipt: "",
                messageText: validJsonMessage,
                dequeueCount: 0);

            httpClientServiceMock.Setup(x => x.GetInitialsAsync("John Doe"))
                .ReturnsAsync("SVGData MOCK Data");

            personServiceMock.Setup(x => x.UpdatePersonSvgAsync(It.IsAny<PersonDto>(), "SVGData MOCK Data"))
                .ReturnsAsync(true);

            var function = new FunctionB(logger, personServiceMock.Object, httpClientServiceMock.Object);

            // Act
            await function.Run(queueMessage);

            // Assert
            Assert.Contains("FunctionB: Updated SVG data for John Doe", logger.Logs);
        }
    }
}
