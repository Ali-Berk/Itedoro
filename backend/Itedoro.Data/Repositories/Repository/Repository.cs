using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Itedoro.Data.Repositories.Repository.Interfaces;

namespace Itedoro.Data.Repositories.Repository;

public class Repository<T>(ItedoroDbContext context) : IRepository<T> where T : class
{
    protected readonly ItedoroDbContext Context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public IQueryable<T> GetQueryable() 
        => _dbSet.AsNoTracking();

    public async Task<T?> GetByIdAsync(Guid id) 
        => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity) 
        => await _dbSet.AddAsync(entity);

    public void Delete(T entity) 
        => _dbSet.Remove(entity);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) 
        => await _dbSet.AnyAsync(predicate);

    public async Task SaveAsync() 
        => await Context.SaveChangesAsync(); 
    public async Task SaveAsync(CancellationToken ct) 
        => await Context.SaveChangesAsync(ct); 
}