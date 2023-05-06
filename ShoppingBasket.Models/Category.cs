using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingBasket.Models;

public class Category
{
    [Key] public int Id { get; set; }

    [Required]
    [DisplayName("Category Name")]
    public string Name { get; set; }

    public DateTime CreatedAt { get; private set; } = DateTime.Now;
}