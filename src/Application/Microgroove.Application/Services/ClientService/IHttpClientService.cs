using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.Application.Services.ClientService
{
    /// <summary>
    /// Http Client for service
    /// </summary>
    public interface IHttpClientService
    {
        /// <summary>
        /// Get initials from external API call via full name
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        Task<string> GetInitialsAsync(string fullName);
    }
}
