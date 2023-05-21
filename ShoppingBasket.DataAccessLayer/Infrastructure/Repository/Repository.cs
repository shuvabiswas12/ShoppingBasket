using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context, DbSet<T> dbSet)
    {
        _context = context;
        _dbSet = dbSet;
    }

    public IEnumerable<T> GetAll(string? includeProperties = null, Expression<Func<T, bool>>? predicate = null)
    {
        throw new NotImplementedException();
    }

    public T GetT(Expression<Func<T, bool>> predicate, string? includeProperties = null)
    {
        throw new NotImplementedException();
    }

    public void Add(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }
}