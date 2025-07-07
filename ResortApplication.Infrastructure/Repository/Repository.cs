using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Infrastructure.Data;

namespace ResortApplication.Infrastructure.Repository;

public class Repository<T>(ApplicationDbContext db) : IRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet = db.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter,
        string? includeProperties = null,
        bool disableTracking = false)
    {
        IQueryable<T> query = _dbSet;
        if (disableTracking)
            query = query.AsNoTracking();
        if (filter is not null)
            query = query.Where(filter);

        if (string.IsNullOrEmpty(includeProperties)) return await query.ToListAsync();

        foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty.Trim());

        return await query.ToListAsync();
    }

    public async Task<T?> GetAsync(
        Expression<Func<T, bool>>? filter,
        string? includeProperties = null,
        bool disableTracking = false)
    {
        IQueryable<T> query = _dbSet;
        if (disableTracking)
            query = query.AsNoTracking();
        if (filter is not null)
            query = query.Where(filter);

        if (!string.IsNullOrEmpty(includeProperties))
            foreach (var includeProperty in includeProperties.Split(new[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty.Trim());

        return await query.FirstOrDefaultAsync();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}