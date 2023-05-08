using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingBasket.Models;

public class Product
{
    [Key] public int Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string Description { get; set; }

    [Required] public string ProductImage_1 { get; set; }

    public string? ProductImage_2 { get; set; }

    public string? ProductImage_3 { get; set; }

    [Required] public double Price { get; set; }

    [ForeignKey(("Category"))] public int CategoryId { get; set; }

    public Category Category { get; set; }

    public DateTime CreatedAt { get; private set; } = DateTime.Now;
}