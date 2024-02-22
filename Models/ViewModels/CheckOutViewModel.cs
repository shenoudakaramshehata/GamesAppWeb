using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models.ViewModels
{
    public class CheckOutViewModel
    {
        //int CustomerId, int CustomerAddressId, int CouponId,int PaymentMehodPaymentMethodId

        public int CustomerId { get; set; }

        public int CustomerAddressId { get; set; }

        public int? CouponId { get; set; }

        public int PaymentMehodPaymentMethodId { get; set; }

    }
}
