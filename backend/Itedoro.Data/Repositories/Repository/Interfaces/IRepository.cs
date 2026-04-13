using System.Linq.Expressions;

namespace Itedoro.Data.Repositories.Repository.Interfaces;

    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetQueryable();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Delete(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task SaveAsync();
        Task SaveAsync(CancellationToken cancellationToken);
        
    }
