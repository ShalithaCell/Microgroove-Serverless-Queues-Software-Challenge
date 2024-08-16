using Microgroove.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
                return (IGenericRepository<TEntity>)_repositories[typeof(TEntity)];

            var repository = new GenericRepository<TEntity>(context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflicts


                throw new Exception("Concurrency conflict occurred", ex);
            }
        }

        public int SaveChanges()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflicts


                throw new Exception("Concurrency conflict occurred", ex);
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
