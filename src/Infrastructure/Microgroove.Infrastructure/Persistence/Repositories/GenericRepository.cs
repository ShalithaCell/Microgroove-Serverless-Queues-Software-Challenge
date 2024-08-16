using Microgroove.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity>(DbContext context) : IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly DbContext _context = context;
        private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        ///<inheritdoc/>
        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        ///<inheritdoc/>
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        ///<inheritdoc/>
        public Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        ///<inheritdoc/>
        public Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).AsNoTracking().ToListAsync();
        }

        ///<inheritdoc/>
        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        ///<inheritdoc/>
        public IQueryable<TEntity> Query()
        {
            return _dbSet.AsQueryable();
        }
    }
}
