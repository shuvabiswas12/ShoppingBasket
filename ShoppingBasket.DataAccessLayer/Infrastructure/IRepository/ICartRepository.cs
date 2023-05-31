using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

public interface ICartRepository : IRepository<Cart>
{
    void Update();
}