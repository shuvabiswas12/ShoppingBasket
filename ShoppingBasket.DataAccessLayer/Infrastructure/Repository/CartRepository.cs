using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.Repository;

public class CartRepository : Repository<Cart>, ICartRepository
{
    private ApplicationDbContext _context;
    
    public CartRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(int cartId)
    {
        throw new NotImplementedException();
    }
}