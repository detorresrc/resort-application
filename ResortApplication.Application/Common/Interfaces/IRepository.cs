using System.Linq.Expressions;

namespace ResortApplication.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter,
        string? includeProperties = null,
        bool disableTracking = false);

    Task<T?> GetAsync(
        Expression<Func<T, bool>>? filter,
        string? includeProperties = null,
        bool disableTracking = false);

    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);

    void DeleteRange(IEnumerable<T> entities);
}