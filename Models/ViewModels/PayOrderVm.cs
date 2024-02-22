namespace Gameapp.Models.ViewModels
{
    public class PayOrderVm
    {
        public int CustomerId { get; set; }
        public int? CouponId { get; set; }
        public int PaymentMehodId{ get; set; }
        public int CustomerAddressId { get; set; }
    }
}
