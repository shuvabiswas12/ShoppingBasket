using Microsoft.EntityFrameworkCore;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer.Infrastructure.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;
    
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Product product)
    {
        var productToUpdate = _context.Products.Include("Stock").FirstOrDefault(p => p.Id == product.Id);
        if (productToUpdate != null)
        {
            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;
            productToUpdate.Description = product.Description;
            productToUpdate.ProductImage_1 = product.ProductImage_1;
            productToUpdate.ProductImage_2 = product.ProductImage_2;
            productToUpdate.ProductImage_3 = product.ProductImage_3;
            productToUpdate.CategoryId = product.CategoryId;
            
            // for stock update through product model.
            // if input value does not match with existing value
            if (productToUpdate.Stock.CurrentStock != product.Stock.CurrentStock)
            {
                productToUpdate.Stock.LastStockInsertedAt = productToUpdate.Stock.NewStockInsertedAt;
                productToUpdate.Stock.NewStockInsertedAt = product.Stock.NewStockInsertedAt;
                productToUpdate.Stock.PreviousStock = productToUpdate.Stock.CurrentStock;
                productToUpdate.Stock.CurrentStock = product.Stock.CurrentStock;
            }
        }
    }
}