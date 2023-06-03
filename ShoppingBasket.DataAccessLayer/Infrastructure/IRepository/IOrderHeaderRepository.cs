using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
    void Update(OrderHeader orderHeader);
}