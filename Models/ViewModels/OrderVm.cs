using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models.ViewModels
{
    public class OrderVm
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderSerial { get; set; }
        public int? CustomerId { get; set; }
        public int? ShippingAddressId { get; set; }
        public string ShippingAddressTitle { get; set; }
        public string Picture { get; set; }
        public string customerAddress { get; set; }
        public string customerPhone { get; set; }
        public string customerEmail { get; set; }

        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopEmail { get; set; }

        public string ShopMobile { get; set; }
        public string PaymentMehodPaymentMethodName { get; set; }
        public string CustomerName { get; set; }

        public string IsDeliverd { get; set; }
      
        public int? OrderIndex { get; set; }
        public int? PaymentMehodPaymentMethodId { get; set; }
        public int ShopId { get; set; }

        public float OrderTotal { get; set; }

        public float OrderDiscount { get; set; }
        public int? CouponId { get; set; }
        public string ispaid { get; set; }


        public int uniqeId { get; set; }
        public float? CouponAmount { get; set; }
        public float? Deliverycost { get; set; }
        public float? OrderNet { get; set; }


        
    }
}
