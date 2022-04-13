using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Core.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>>GetByIdAsync(int id);
        Task<T> DeleteAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> FilterAsync(Expression<Func<T, bool>> expression);

    }
}
