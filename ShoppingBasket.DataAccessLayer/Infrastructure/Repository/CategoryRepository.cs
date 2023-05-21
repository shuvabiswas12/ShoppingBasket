using Microsoft.EntityFrameworkCore;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public void Update(Category category)
    {
        var categoryToUpdate = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
        if (categoryToUpdate != null)
        {
            categoryToUpdate.Name = category.Name;
        }
    }

    
}