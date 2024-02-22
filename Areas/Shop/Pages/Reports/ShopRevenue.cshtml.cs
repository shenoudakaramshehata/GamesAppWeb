using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Gameapp.Models;
using Gameapp.Reports;
using Gameapp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Gameapp.Areas.Shop.Pages.Reports
{
    public class ShopRevenueModel : PageModel
    {
        public GamesContext _context { get; }
        UserManager<ApplicationUser> UserManger;
        public ShopRevenueModel(GamesContext context, UserManager<ApplicationUser> userManger)
        {
            _context = context;
            UserManger = userManger;

        }

        public rptshoprevenue Report { get; set; }
        [BindProperty]
        public FilterModel filterModel { get; set; }

        public void OnGet()
        {
            Report = new rptshoprevenue();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            List<OrderVm> ds = _context.Order.Include(a => a.ShippingAddress).Include(a => a.shop).Include(a => a.Customer).Include(a => a.PaymentMehodPaymentMethod).Where(e=>e.ShopId== user.EntityId).Select(i => new OrderVm
            {
                OrderId = i.OrderId,
                OrderSerial = i.OrderSerial,
                CouponAmount = i.CouponAmount,
                CustomerName = i.Customer.CustomernameEn,
                OrderNet = i.OrderNet,
                OrderDate = i.OrderDate,
                CustomerId = i.CustomerId,
                OrderDiscount = i.OrderDiscount,
                CouponId = i.CouponId,
                Deliverycost = i.Deliverycost,
                IsDeliverd = i.IsDeliverd == true ? "Deliverd" : "Not Deliverd",
                OrderIndex = i.OrderIndex,
                OrderTotal = i.OrderTotal,
                Picture=i.shop.Pic,
                customerAddress=i.Customer.CustomerAddress,
                customerEmail=i.Customer.CustomerEmail,
                customerPhone=i.Customer.Customerphone,
                ShopEmail=i.shop.Email,
                ShopMobile=i.shop.Mobile,
                ShopAddress=i.shop.Address,

                PaymentMehodPaymentMethodId = i.PaymentMehodPaymentMethodId,
                PaymentMehodPaymentMethodName = i.PaymentMehodPaymentMethod.PaymentMethodName,
                ShippingAddressId = i.ShippingAddressId,
                ispaid = i.ispaid == true ? "Paid" : "Not Paid",

                ShippingAddressTitle = i.ShippingAddress.AddressNickname,
                ShopId = i.ShopId,
                ShopName = i.shop.ShopTlen,
                uniqeId = i.uniqeId,


            }).ToList();

            if (filterModel.CustomterId != null)
            {
                ds = ds.Where(i => i.CustomerId == filterModel.CustomterId).ToList();
            }
            if (filterModel.ShippingAddressId != null)
            {
                ds = ds.Where(i => i.ShippingAddressId == filterModel.ShippingAddressId).ToList();
            }
            if (filterModel.radiobtn != null)
            {
                if (filterModel.radiobtn == "OnDay")
                {
                    if (filterModel.OnDay != null)
                    {
                        ds = ds.Where(i => i.OrderDate.Date == filterModel.OnDay.Value.Date).ToList();
                    }
                    else
                    {
                        ds = null;
                    }
                }
                else if (filterModel.radiobtn == "Period")
                {
                    if (filterModel.FromDate != null && filterModel.ToDate == null)
                    {
                        ds = null;
                    }
                    if (filterModel.FromDate == null && filterModel.ToDate != null)
                    {
                        ds = null;
                    }
                    if (filterModel.FromDate != null && filterModel.ToDate != null)

                    {
                        ds = ds.Where(i => i.OrderDate.Date >= filterModel.FromDate.Value.Date && i.OrderDate <= filterModel.ToDate.Value.Date).ToList();
                    }
                }
            }
            if (filterModel.radiobtn == null && (filterModel.OnDay != null || filterModel.FromDate != null || filterModel.ToDate != null))
            {
                ds = null;
            }

            if (filterModel.CustomterId == null && filterModel.ShippingAddressId == null && filterModel.FromDate == null && filterModel.ToDate == null && filterModel.radiobtn == null)
            {
                ds = null;
            }

            Report = new rptshoprevenue();
            Report.DataSource = ds;
            return Page();

        }
    }
}
