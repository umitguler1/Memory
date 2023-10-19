using Memory.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Memory.Core.DataAccess
{
    public interface IRepository<T> where T : class,IEntity,new()
    {
        Task<T> GetAsync(Expression<Func<T,bool>> expression);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
        Task<int> HardResetAsync(T entity);
    }
}
