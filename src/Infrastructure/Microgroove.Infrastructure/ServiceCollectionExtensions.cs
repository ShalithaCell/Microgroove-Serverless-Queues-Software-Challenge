using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microgroove.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microgroove.Domain.Repositories;
using Microgroove.Infrastructure.Persistence.Repositories;
using Microsoft.Data.Sqlite;

namespace Microgroove.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterApplicationInfrastructure(this IServiceCollection services)
        {
            var keepAliveConnection = new SqliteConnection("DataSource=:memory:");
            keepAliveConnection.Open();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(keepAliveConnection);
            });

            #region Ensure database is created

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.OpenConnection();
                dbContext.Database.EnsureCreated();
            }

            #endregion

            // Register the Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }
    }
}
