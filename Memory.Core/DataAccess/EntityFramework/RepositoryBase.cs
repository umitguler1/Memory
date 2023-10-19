using Memory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Memory.Core.DataAccess.EntityFramework
{
    public class RepositoryBase<T> : IRepository<T> where T : class, IEntity, new()
    {
        public DbContext _db;
        public DbSet<T> _dbSet;
        public RepositoryBase(DbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public Task<int> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            return _db.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(T entity)
        { 
            entity.IsDeleted = true;
            return UpdateAsync(entity);
        }

        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null)
        {
            return expression == null ? _dbSet.Where(x => x.IsDeleted == false).ToListAsync() :
                _dbSet.Where(expression) // Tamamını getirir elimize liste gelir.
                .Where(x => x.IsDeleted == false)// Gelen listeden silinmemiş olanları isteriz.
                .ToListAsync();
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public Task<int> HardResetAsync(T entity)
        {
            _dbSet.Remove(entity);
            return _db.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return _db.SaveChangesAsync();
        }
    }
}
