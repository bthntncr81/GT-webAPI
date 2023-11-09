using GTBack.Core.DTO;
using GTBack.Core.Entities;
using System.Linq.Expressions;

namespace GTBack.Core.Services
{
    public interface IService<T> where T : class
    {
        Task<T> GetByIdAsync(Expression<Func<T, bool>> expression);


        Task<IEnumerable<T>> GetAllAsync();

        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        Task<T> AddAsync(T entity);
        Task SendMail(MailData mail);
        Task<IEnumerable<T>>AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);

        Task RemoveRangeAsync(IEnumerable<T> entities);

        Task<T?> FindAsNoTrackingAsync(Expression<Func<T, bool>> expression);



        Task<T?> FindAsync(Expression<Func<T, bool>> expression);

        void Remove(Expression<Func<T, bool>> expression);


    }
}
