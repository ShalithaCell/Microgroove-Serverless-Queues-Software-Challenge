using Microgroove.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.Application.Services.PersonService
{
    /// <summary>
    /// Person Data Related Operations
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// Retrieve the person data by first name and last name
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task<PersonDetailDto?> GetPersonByNameAsync(PersonDto person);

        /// <summary>
        /// Add Person Data the Database
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task<bool> AddPersonAsync(PersonDto person);

        /// <summary>
        /// Check if the person is already exists in the database
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task<bool> PersonExistsAsync(PersonDto person);

        /// <summary>
        /// Update the person SVG data
        /// </summary>
        /// <param name="person"></param>
        /// <param name="svgData"></param>
        /// <returns></returns>
        Task UpdatePersonSvgAsync(PersonDto person, string svgData);
    }
}
