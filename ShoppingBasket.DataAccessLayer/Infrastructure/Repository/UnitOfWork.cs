using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}