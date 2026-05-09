using ECommerce.Domain.Entities;
using ECommerce.persistence;
using ECommerce.persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _dbContext;
        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity) => await _dbContext.AddAsync(entity);


        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id) => await _dbContext.Set<TEntity>().FindAsync(id);
        public void Remove(TEntity entity) => _dbContext.Remove(entity);

        public void Update(TEntity entity) => _dbContext.Update(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification)
        {
            // Entry Point = _dbContext.Set<TEntity>().AsQueryable() IQueryable<TEntity>
            var EntryPoint = _dbContext.Set<TEntity>().AsQueryable();

            var query = SpecificationEvaluator.CreateQuery<TEntity, TKey>(EntryPoint, specification);
            // Finally, we execute the query and return the results as a list
            return await query.ToListAsync();

        }

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification)
        {
            var EntryPoint = _dbContext.Set<TEntity>().AsQueryable();

            var query = SpecificationEvaluator.CreateQuery<TEntity, TKey>(EntryPoint, specification);

            // Finally, we execute the query and return the result
            return await query.FirstOrDefaultAsync();
        }
        public async Task<int> CountAsync(ISpecification<TEntity, TKey> specification)
        {
            //var EntryPoint = _dbContext.Set<TEntity>().AsQueryable();
            var count = await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification).CountAsync();
            Console.WriteLine($"{count}####################");
            // Finally, we execute the query and return the count
            return count;
        }
    }
}
