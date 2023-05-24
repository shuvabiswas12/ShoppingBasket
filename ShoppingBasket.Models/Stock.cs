using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingBasket.Models;

public class Stock
{
    [Key] public int Id { get; set; }

    [DisplayName("Current Stock")] [Required] public int CurrentStock { get; set; } = 0;

    [DisplayName("Previous Stock")] public int PreviousStock { get; set; } = 0;

    public Product Product { get; set; }

    [ForeignKey("Product")] public int ProductId { get; set; }

    [DisplayName("Last Stock Inserted At")]
    public DateTime? LastStockInsertedAt { get; set; }

    [DisplayName("New Stock Inserted At")] public DateTime NewStockInsertedAt { get; set; } = DateTime.Now;
}