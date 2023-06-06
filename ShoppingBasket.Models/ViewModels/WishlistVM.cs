using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Models.ViewModels
{
	public class WishlistVM
	{
		public IEnumerable<Wishlist> Wishlists { get; set; } = null!;
	}
}
