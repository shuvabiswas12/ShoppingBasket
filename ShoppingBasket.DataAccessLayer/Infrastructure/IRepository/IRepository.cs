using System.Linq.Expressions;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll(string? includeProperties = null, Expression<Func<T, bool>>? predicate = null);
    T GetT(Expression<Func<T, bool>> predicate, string? includeProperties = null);
    void Add(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}