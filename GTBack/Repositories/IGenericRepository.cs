using GTBack.Core.DTO;
using GTBack.Core.Entities;
using System.Linq.Expressions;

namespace GTBack.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Expression<Func<T, bool>> expression);


        IQueryable<T> GetAll();

        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        Task<T> AddAsync(T entity);
    

        Task AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        void Remove(T entity);

        void RemoveRange (IEnumerable<T> entities);
        Task<T?> FindAsNoTrackingAsync(Expression<Func<T, bool>> expression);


        Task<T?> FindAsync(Expression<Func<T, bool>> expression);

        void Remove(Expression<Func<T, bool>> expression);


    }
}
