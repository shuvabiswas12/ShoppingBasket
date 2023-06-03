namespace ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    
    IProductRepository ProductRepository { get; }
    
    IStockRepository StockRepository { get; }
    
    ICartRepository CartRepository { get; }
    
    IWishlistRepository WishlistRepository { get; }
    
    IOrderHeaderRepository OrderHeaderRepository { get; }
    
    IOrderDetailsRepository OrderDetailsRepository { get; }
    
    void Save();
}