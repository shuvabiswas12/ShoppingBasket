using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<Stock> Stocks { get; set; } = null!;

    public DbSet<Wishlist> Wishlists { get; set; } = null!;

    public DbSet<Cart> Carts { get; set; }

    public DbSet<Coupon> Coupons { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    // Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}