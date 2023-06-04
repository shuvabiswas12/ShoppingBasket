using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.Repository;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderHeaderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(OrderHeader orderHeader)
    {
        throw new NotImplementedException();
    }

    public void UpdateStatus(int orderHeaderId, string orderStatus, string? paymentStatus = null)
    {
        var orderHeaderToUpdate = _context.OrderHeaders.FirstOrDefault(o => o.Id == orderHeaderId);
        
        if (orderHeaderToUpdate == null) return;
        
        orderHeaderToUpdate.OrderStatus = orderStatus;
            
        if (paymentStatus != null)
        {
            orderHeaderToUpdate.PaymentStatus = paymentStatus;
        }
    }

    public void PaymentStatus(int orderHeaderId, string? sessionId = null, string? paymentIntentId = null)
    {
        var orderHeaderToUpdate = _context.OrderHeaders.FirstOrDefault(o => o.Id == orderHeaderId);
        if (orderHeaderToUpdate != null)
        {
            if (sessionId != null) { 
                orderHeaderToUpdate.SessionId = sessionId;
                orderHeaderToUpdate.PaymentDate = DateTime.Now;
            }
            else orderHeaderToUpdate.PaymentIntentId = paymentIntentId;
            
        }
    }
}