using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Models
{
    public class OrderDetail
    {
        [Key] public int Id { get; set; }

        [ForeignKey("OrderHeader")] public int OrderHeaderId { get; set; }

        [ValidateNever] public OrderHeader OrderHeader { get; set; }

        [ForeignKey("Product")] public int ProductId { get; set; }

        [ValidateNever] public Product Product { get; set; }

        [Required] public double Price { get; set; }

        [Required] public int Count { get; set; }
    }
}
