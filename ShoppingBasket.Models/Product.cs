using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShoppingBasket.Models;

public class Product
{
    [Key] public int Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string Description { get; set; }

    [DisplayName("Product Images")]
    [Required]
    public string ProductImage_1 { get; set; }

    public string? ProductImage_2 { get; set; } = null;

    public string? ProductImage_3 { get; set; } = null;

    [Required] public double Price { get; set; }

    [ForeignKey(("Category"))]
    [DisplayName("Category")]
    public int CategoryId { get; set; }

    [ValidateNever] public Category Category { get; set; }
    
    [ValidateNever] public Stock? Stock { get; set; }

    [ValidateNever] public Wishlist? Wishlist { get; set; }

    [DisplayName("Product Entry Date")]
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
}