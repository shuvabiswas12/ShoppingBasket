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

    public IOrderHeaderRepository OrderHeaderRepository { get; }

    public IOrderDetailsRepository OrderDetailsRepository { get; }

    public IContactRepository ContactRepository { get; }

    public IApplicationUserRepository ApplicationUserRepository { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        CartRepository = new CartRepository(_context);
        WishlistRepository = new WishlistRepository(_context);
        CategoryRepository = new CategoryRepository(context);
        ProductRepository = new ProductRepository(context);
        StockRepository = new StockRepository(context);
        OrderHeaderRepository = new OrderHeaderRepository(_context);
        OrderDetailsRepository = new OrderDetailsRepository(_context);
        ContactRepository = new ContactRepository(_context);
        ApplicationUserRepository = new ApplicationUserRepository(_context);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}