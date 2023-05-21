using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Stock> Stocks { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    // Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}