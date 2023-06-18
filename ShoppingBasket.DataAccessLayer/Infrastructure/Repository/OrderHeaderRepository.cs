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
        var order = _context.OrderHeaders.FirstOrDefault(o => o.Id == orderHeader.Id);
        if (order == null) return;

        order.TrackingNumber = orderHeader.TrackingNumber ?? null;
        order.Carrier = orderHeader.Carrier ?? null;
        order.ShippingDate = orderHeader.TrackingNumber != null ? DateTime.Now : null;
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
            if (sessionId != null)
            {
                orderHeaderToUpdate.SessionId = sessionId;
            }
            else { 
                orderHeaderToUpdate.PaymentIntentId = paymentIntentId;
                orderHeaderToUpdate.PaymentDate = DateTime.Now;
            }

        }
    }
}