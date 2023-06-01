using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Models.ViewModels
{
    public class CartVM
    {
        public Cart? Cart { get; set; }
        public Product Product { get; set; } = null!;
    }
}
