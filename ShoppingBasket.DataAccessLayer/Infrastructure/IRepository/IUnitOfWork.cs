namespace ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    
    IProductRepository ProductRepository { get; }
    
    IStockRepository StockRepository { get; }
    void Save();
}