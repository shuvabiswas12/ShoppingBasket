using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingBasket.Models;

namespace ShoppingBasket.DataAccessLayer;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<Stock> Stocks { get; set; } = null!;

    public DbSet<Wishlist> Wishlists { get; set; } = null!;

    public DbSet<Cart> Carts { get; set; } = null!;

    public DbSet<Coupon> Coupons { get; set; } = null!;

    public DbSet<OrderHeader> OrderHeaders { get; set; } = null!;

    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;

    public DbSet<Contact> Contacts { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    // Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set the OrderNumber column to unique by Fluent API
        modelBuilder.Entity<OrderHeader>()
            .HasIndex(o => o.OrderNumber)
            .IsUnique();
    }
}