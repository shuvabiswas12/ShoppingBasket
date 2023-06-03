using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

public interface IOrderDetailsRepository : IRepository<OrderDetail>
{
    void Update(OrderDetail orderDetail);
}