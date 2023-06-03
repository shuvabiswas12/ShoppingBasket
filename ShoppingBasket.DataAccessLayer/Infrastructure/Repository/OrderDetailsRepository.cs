using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.Repository;

public class OrderDetailsRepository : Repository<OrderDetail>, IOrderDetailsRepository
{
    private readonly ApplicationDbContext _context;
    
    public OrderDetailsRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(OrderDetail orderDetail)
    {
        throw new NotImplementedException();
    }
}