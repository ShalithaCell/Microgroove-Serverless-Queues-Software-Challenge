using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microgroove.Application.Services.ClientService;
using Microgroove.Application.Services.PersonService;
using Microgroove.Application.Validators;
using Microgroove.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microgroove.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterApplication(this IServiceCollection services)
        {

            // Register Infrastructure Layer
            services.RegisterApplicationInfrastructure();

            // Register Core Services
            services.AddScoped<IPersonService, PersonService>();

            // Validators
            services.AddTransient<PersonDtoValidator>();

            // Inject Http Client Service
            services.AddHttpClient<IHttpClientService, HttpClientService>();
        }
    }
}
