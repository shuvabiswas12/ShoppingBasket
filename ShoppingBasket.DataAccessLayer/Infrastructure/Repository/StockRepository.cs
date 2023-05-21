using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.Repository;

public class StockRepository : Repository<Stock>, IStockRepository
{
    private ApplicationDbContext _context;
    
    public StockRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Stock stock)
    {
        throw new NotImplementedException();
    }
}