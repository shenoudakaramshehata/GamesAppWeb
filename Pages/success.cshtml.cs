using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Gameapp.Data;
using Gameapp.Models;
using Microsoft.EntityFrameworkCore;

namespace Gameapp.Pages
{
    public class successModel : PageModel
    {
        private GamesContext _context;
        public List<Order> order { get; set; }

        private readonly IEmailSender _emailSender;

        public successModel(GamesContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> OnGetAsync(string payment_type,string PaymentID,string Result,int OrderID,DateTime? PostDate,string TranID,
        string Ref, string TrackID, string Auth)
        {
            Coupon coupon = null;

            if (OrderID != 0)
            {
                order =await _context.Order.Where(e => e.uniqeId == OrderID).ToListAsync();

                foreach (var item in order)
                {
                    item.ispaid = true;
                    item.payment_type = payment_type;
                    item.PaymentID = PaymentID;
                    item.Result = Result;
                    item.uniqeId = OrderID;
                    item.PostDate = PostDate;
                    item.TranID = TranID;
                    item.Ref = Ref;
                    item.TrackID = TrackID;
                    item.Auth = Auth;
                    if (item.CouponId != null)
                    {
                        coupon = _context.Coupon.FirstOrDefault(c => c.Id == item.CouponId);

                        if (coupon != null)
                        {
                            coupon.Used = true;
                            var UpdatedCoupon = _context.Coupon.Attach(coupon);
                            UpdatedCoupon.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }
                    var UpdatedOrder = _context.Order.Attach(item);
                    UpdatedOrder.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    _context.SaveChanges();

                    var Customer = _context.Customer.Find(item.CustomerId);
                    if (item.CustomerId != null)
                    {
                        var carts = _context.ShoppingCart.Where(e => e.CustomerId == item.CustomerId);
                        _context.ShoppingCart.RemoveRange(carts);
                        _context.SaveChanges();
                    }
                }
                
            }
            return Page();

        }
    }
}
