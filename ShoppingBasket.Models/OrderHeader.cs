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
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")] public string ApplicationUserId { get; set; } = null!;
        [ValidateNever] public ApplicationUser ApplicationUser { get; set; }

        [DisplayName("Order Date")] public DateTime OrderDate { get; set; } = DateTime.Now;

        [DisplayName("Shipping Date")] public DateTime? ShippingDate { get; set; }

        [DisplayName("Payment Date")] public DateTime? PaymentDate { get; set; }

        [Required, DisplayName("Order Status")] public string OrderStatus { get; set; } = null!;

        [Required, DisplayName("Payment Status")] public string PaymentStatus { get; set; } = null!;

        [DisplayName("Tracking Number")] public string? TrackingNumber { get; set; }
        
        public string? Carrier { get; set; }
        
        [DisplayName("Session ID")] public string? SessionId { get; set; }
        
        [DisplayName("Payment Intent ID")] public string? PaymentIntentId { get; set; }

        [Required] public string FirstName { get; set; } = null!;
        [Required] public string LastName { get; set; } = null!;
        
        [Required] public string Address { get; set; } = null!;
        
        [Required] public string Phone { get; set; } = null!;
        
        [Required] public string Email { get; set; } = null!;
        
        [Required] public string City { get; set; } = null!;

        [Required] public string Country { get; set; } = null!;
        
        [Required] public string State { get; set; } = null!;
        
        [Required, DisplayName("Post Code")] public string PostCode { get; set; } = null!;

        [Required, DisplayName("Payment Type")] public string PaymentType { get; set; } = null!;
    }
}
