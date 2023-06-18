using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Models.ViewModels
{
    public class OrderShippingVM
    {
        [Required] public int OrderId { get; set; }
        [Required] public string Carrier { get; set; } = null!;
        [Required, Display(Name = "Tracking Number")] public string TrackingNumber { get; set; } = null!;
    }
}
