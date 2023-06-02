using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [Required, DisplayName("Discount Code"), MaxLength(6, ErrorMessage = "Code length should be in 6 digits!"), MinLength(4, ErrorMessage = "Min. Code length at least 4 digits!")]
        public string CouponCode { get; set; } = null!;

        [Required, DisplayName("Coupon Name")]
        public string CouponName { get; set; } = null!;

        [Required, DisplayName("Coupon Rate"), MinLength(5, ErrorMessage = "Min. coupon rate should be >= 5%"), MaxLength(70, ErrorMessage = "Max. coupon rate should be <= 70%")]
        public double CouponRate { get; set; }

        [Required, DisplayName("Min. Ordered Amount")]
        public double MinOrderedAmount { get; set; } = 0.0;

        [DisplayName("Max. Ordered Amount")]
        public double? MaxOrderedAmount { get; set; }

        [DisplayName("Expire Date (UpTo)")]
        public DateTime ExpireDate { get; set; } = DateTime.Now.AddDays(2);

    }
}
