using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Cart> Carts { get; set; } = null!;
        public double Total { get; set; } = 0.0;
    }
}
