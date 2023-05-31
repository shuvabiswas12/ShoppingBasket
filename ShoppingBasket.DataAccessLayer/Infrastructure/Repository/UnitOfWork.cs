using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public ICategoryRepository CategoryRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IStockRepository StockRepository { get; }
    
    public ICartRepository CartRepository { get; }
    
    public IWishlistRepository WishlistRepository { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        CartRepository = new CartRepository(_context);
        WishlistRepository = new WishlistRepository(_context);
        CategoryRepository = new CategoryRepository(context);
        ProductRepository = new ProductRepository(context);
        StockRepository = new StockRepository(context);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}