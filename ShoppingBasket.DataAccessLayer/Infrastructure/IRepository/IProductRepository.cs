using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

public interface IProductRepository : IRepository<Product>
{
    void Update(Product product);
}