using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.CommonHelper
{
    public class OrderStatus
    {
        public const string STATUS_PENDING = "Pending";
        public const string STATUS_APPROVED = "Approved";
        public const string STATUS_CANCELLED = "Cancelled";
        public const string STATUS_REFUNDED = "Refunded";
        public const string STATUS_PROCESSING = "Processing";
        public const string STATUS_SHIPPED = "Shipped";
    }
}
