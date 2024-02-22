using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models.ViewModels
{
    public class OrderModel
    {
        public int CustomerId { get; set; }
        public int ShopId { get; set; }
        public int AdressId { get; set; }
        public string PaymentMethodName { get; set; }
        public string CouponSerial { get; set; }
        public float? OrderNet { get; set; }

    }
}
