using System.ComponentModel.DataAnnotations;

namespace ShoppingBasket.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
}