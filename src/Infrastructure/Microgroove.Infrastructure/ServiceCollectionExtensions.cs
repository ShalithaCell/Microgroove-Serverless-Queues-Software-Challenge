using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microgroove.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Microgroove.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterApplicationInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=:memory:")); // In-memory SQLite

            #region Ensure database is created

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.OpenConnection();
                dbContext.Database.EnsureCreated();
            }

            #endregion

        }
    }
}
