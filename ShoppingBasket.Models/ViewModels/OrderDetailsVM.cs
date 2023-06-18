using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Models.ViewModels
{
    public class OrderDetailsVM
    {
        [ValidateNever] public OrderHeader OrderHeader { get; set; } = null!;
        [ValidateNever] public IEnumerable<OrderDetail> OrderDetail { get; set; } = null!;
        public OrderShippingVM OrderShipping { get; set; } = new OrderShippingVM();
    }
}
