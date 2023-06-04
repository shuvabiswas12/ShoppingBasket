using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
    void Update(OrderHeader orderHeader);
    void UpdateStatus(int orderHeaderId, string orderStatus, string? paymentStatus = null);
    
    void PaymentStatus(int orderHeaderId, string? sessionId = null, string? paymentIntentId = null);
}