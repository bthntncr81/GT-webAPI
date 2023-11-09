using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using GTBack.Core.UnitOfWorks;

namespace GTBack.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;


        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            
            await _context.SaveChangesAsync();

            return entity;
        }
        
      

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AsNoTracking().SingleOrDefaultAsync(expression);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public  T Update(T entity)
        {
            _dbSet.Update(entity);
                      
             _context.SaveChanges();

            return entity;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public async Task<T?> FindAsNoTrackingAsync(Expression<Func<T, bool>> expression)
        {
            return await _context
                .Set<T>()
                .Where(expression)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>()
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public void Remove(Expression<Func<T, bool>> expression)
        {
            _context.Set<T>().RemoveRange(_context.Set<T>().Where(expression));
        }
    }
}