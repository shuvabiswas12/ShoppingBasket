using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category category);
}