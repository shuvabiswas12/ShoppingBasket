using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

public interface IStockRepository : IRepository<Stock>
{
    void Update(Stock stock);
}