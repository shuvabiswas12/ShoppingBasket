using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingBasket.CommonHelper;

namespace ShoppingBasket.Models.ViewModels
{
    public class CheckoutVM
    {
        [ValidateNever] public IEnumerable<Cart> Carts { get; set; } = null!;
        
        public OrderHeader OrderHeader { get; set; } = null!;
        
        public string CashOnDelivery = PaymentTypes.CashOnDelivery;
        
        public string PaymentOnline = PaymentTypes.PaymentOnline;
    }
}
