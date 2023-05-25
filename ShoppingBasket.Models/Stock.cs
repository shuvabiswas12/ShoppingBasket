using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShoppingBasket.Models;

public class Stock
{
    [Key] public int Id { get; set; }

    [DisplayName("Current Stock")] [Required] public int CurrentStock { get; set; } = 0;

    [DisplayName("Previous Stock")] public int PreviousStock { get; set; } = 0;

    [ValidateNever] public Product Product { get; set; }

    [ForeignKey("Product")] public int ProductId { get; set; }

    [DisplayName("Previous Stock Entry Date")]
    public DateTime? LastStockInsertedAt { get; set; }

    [DisplayName("Latest Stock Entry Date")] public DateTime NewStockInsertedAt { get; set; } = DateTime.Now;
}