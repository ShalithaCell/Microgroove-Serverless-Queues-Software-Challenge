using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microgroove.Application.DTOs;
using Microgroove.Domain.Entities;
using Microgroove.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Microgroove.Application.Services.PersonService
{
    /// <inheritdoc />
    public class PersonService(IUnitOfWork unitOfWork) : IPersonService
    {
        /// <inheritdoc />
        public async Task<PersonDetailDto?> GetPersonByNameAsync(PersonDto person)
        {
            PersonDetailDto? personDetail = null;

            var personData = await unitOfWork.Repository<Person>().FirstOrDefaultAsync(p => p.FirstName.ToLower() == person.FirstName.ToLower() &&
                p.LastName.ToLower() == person.LastName.ToLower());

            if (personData != null)
            {
                personDetail = new PersonDetailDto
                {
                    FirstName = personData.FirstName,
                    LastName = personData.LastName,
                    Id = personData.Id,
                    SvgData = personData.SvgData
                };
            }

            return personDetail;
        }

        /// <inheritdoc />
        public async Task<bool> AddPersonAsync(PersonDto personDto)
        {
            if (await PersonExistsAsync(personDto))
            {
                return false; // Duplicate
            }

            var person = new Person
            {
                FirstName = personDto.FirstName,
                LastName = personDto.LastName
            };

            await unitOfWork.Repository<Person>().AddAsync(person);
            await unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> PersonExistsAsync(PersonDto person)
        {
            var personData = await GetPersonByNameAsync(person);

            return personData != null;
        }

        /// <inheritdoc />
        public async Task UpdatePersonSvgAsync(PersonDto person, string svgData)
        {
            var personData = await GetPersonByNameAsync(person);

            if (personData == null)
            {
                return;
            }

            personData.SvgData = svgData;
            await unitOfWork.SaveChangesAsync();
        }
    }
}
