using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Models
{
    public class Wishlist
    {
        [Key] public int Id { get; set; }

        [ValidateNever] public Product Product { get; set; }

        [ForeignKey("Product")] public int ProductId { get; set; }

        public string ApplicationUserId { get; set; }

        [ValidateNever] public ApplicationUser? ApplicationUser { get; set; }

        [DisplayName("Created At")] public DateTime CreatedAt { get; set; }
    }
}