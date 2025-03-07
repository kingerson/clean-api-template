namespace MsClean.Infrastructure;

using MsClean.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

public interface IRepository<T> where T : Entity
{
    Task<T?> GetById(Guid id);
    IQueryable<T> FindQueryable(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    Task<List<T>> FindListAsync(Expression<Func<T, bool>>? expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default);
    Task<List<T>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> expression, string includeProperties);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> expression);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicado = null,
                                            List<Expression<Func<T, object>>>? includes = null,
                                            bool disableTracking = true);
    Task<T> Add(T entity);
    Task AddRange(List<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    void DeleteRange(List<T> entities);
}
