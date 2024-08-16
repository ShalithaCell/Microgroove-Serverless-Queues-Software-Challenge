using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microgroove.Function.PersonNameService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microgroove.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Queues;
using Microgroove.Application.Services.PersonService;
using Microgroove.Application.Validators;
using Microgroove.Application.DTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Azure;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microgroove.Domain.Entities;
using Microgroove.Domain.Repositories;
using System.Linq.Expressions;

namespace Microgroove.Function.Test
{
    public class FunctionATests
    {
        [Fact]
        public async Task RunReturnsBadRequestWhenValidationFails()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<FunctionA>>();
            var personServiceMock = new Mock<IPersonService>();
            var queueClientMock = new Mock<QueueClient>("UseDevelopmentStorage=true", "person-queue");

            var validator = new PersonDtoValidator();

            var function = new FunctionA(loggerMock.Object, personServiceMock.Object, queueClientMock.Object, validator);

            var validJson = JsonConvert.SerializeObject(new PersonDto { FirstName = "", LastName = "Doe" });
            var request = new DefaultHttpContext().Request;
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes(validJson));

            // Act
            var result = await function.Run(request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.NotNull(result.Value);

            var errors = result.Value as IList<ValidationFailure>;
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Equal("FirstName is required.", errors[0].ErrorMessage);
        }


        [Fact]
        public async Task AddPersonAsyncReturnsFalseWhenPersonAlreadyExists()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repositoryMock = new Mock<IGenericRepository<Person>>();

            var existingPerson = new Person { FirstName = "John", LastName = "Doe" };
            repositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(existingPerson);

            unitOfWorkMock.Setup(u => u.Repository<Person>()).Returns(repositoryMock.Object);

            var personService = new PersonService(unitOfWorkMock.Object);

            var personDto = new PersonDto { FirstName = "John", LastName = "Doe" };

            // Act
            var result = await personService.AddPersonAsync(personDto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddPersonAsyncReturnsTrueWhenPersonDoesNotExist()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repositoryMock = new Mock<IGenericRepository<Person>>();

            // Setup the repository to return null, meaning the person does not exist
            repositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync((Person)null);

            unitOfWorkMock.Setup(u => u.Repository<Person>()).Returns(repositoryMock.Object);

            var personService = new PersonService(unitOfWorkMock.Object);

            var personDto = new PersonDto { FirstName = "Jane", LastName = "Doe" };

            // Act
            var result = await personService.AddPersonAsync(personDto);

            // Assert
            Assert.True(result);

            // Verify that AddAsync was called
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Once);

            // Verify that SaveChangesAsync was called
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
