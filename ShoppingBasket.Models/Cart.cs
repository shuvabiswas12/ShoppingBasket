using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShoppingBasket.Models;

public class Cart
{
    [Key] public int Id { get; set; }

    [ForeignKey(("Product"))] public int ProductId { get; set; }

    [ValidateNever] public Product? Product { get; set; }

    public int Count { get; set; } = 1;

    [ForeignKey("ApplicationUser")] public string ApplicationUserId { get; set; }

    [ValidateNever] public ApplicationUser? ApplicationUser { get; set; }
}