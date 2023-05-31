using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.Repository;

public class WishlistRepository : Repository<Wishlist>, IWishlistRepository
{
    private ApplicationDbContext _context;
    
    public WishlistRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}