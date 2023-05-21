using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.Repository;

public class StockRepository : Repository<Stock>, IStockRepository
{
    private readonly ApplicationDbContext _context;
    
    public StockRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Stock stock)
    {
        var stockToUpdate = _context.Stocks.FirstOrDefault(s => s.Id == stock.Id);
        if (stockToUpdate != null)
        {
            stockToUpdate.PreviousStock = stockToUpdate.CurrentStock;
            stockToUpdate.CurrentStock = stock.CurrentStock;
        }
    }
}