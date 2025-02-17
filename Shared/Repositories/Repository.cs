using Microsoft.EntityFrameworkCore;
using Shared.Context;
using Shared.Interfaces;

namespace Shared.Repositories
{
    public class Repository<T>(HahnAppContext context) : IRepository<T> where T : class
    {
        protected readonly HahnAppContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        // Get all entities
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Add new entity
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Upsert method
        public async Task UpsertRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                var propertyInfo = entity.GetType().GetProperty("Id");
                if (propertyInfo != null)
                {
                    var idValue = propertyInfo.GetValue(entity);
                    var existingEntity = await _dbSet.FindAsync(idValue);
                    // Add if does not exist
                    if (existingEntity == null)
                        await _dbSet.AddAsync(entity);
                    // Update if exists
                    else
                        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                }
                else
                    await _dbSet.AddAsync(entity);
            }
            await _context.SaveChangesAsync();
        }
    }
}