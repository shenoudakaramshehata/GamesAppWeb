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

namespace Gameapp.Areas.Admin.Pages.Reports
{
    public class RevenueModel : PageModel
    {
        public GamesContext _context { get; }
        public RevenueModel(GamesContext context)
        {
            _context = context;
        }

        public rptrevenue Report { get; set; }
        [BindProperty]
        public FilterModel filterModel { get; set; }

        public void OnGet()
        {
            Report = new rptrevenue();
        }
        public IActionResult OnPost()
        {

            List<OrderVm> ds = _context.Order.Include(a => a.ShippingAddress).Include(a => a.shop).Include(a => a.Customer).Include(a => a.PaymentMehodPaymentMethod).Select(i => new OrderVm
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

                PaymentMehodPaymentMethodId = i.PaymentMehodPaymentMethodId,
                PaymentMehodPaymentMethodName = i.PaymentMehodPaymentMethod.PaymentMethodName,
                ShippingAddressId = i.ShippingAddressId,
                ispaid = i.ispaid == true ? "Paid" : "Not Paid",

                ShippingAddressTitle = i.ShippingAddress.AddressNickname,
                ShopId = i.ShopId,
                ShopName = i.shop.ShopTlen,
                uniqeId = i.uniqeId,


            }).ToList();

            if (filterModel.ShopId != null)
            {
                ds = ds.Where(i => i.ShopId == filterModel.ShopId).ToList();
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
            if(filterModel.radiobtn == null &&(filterModel.OnDay != null|| filterModel.FromDate != null || filterModel.ToDate != null ))
            {
                ds = null;
            }

            if (filterModel.ShopId == null && filterModel.ShippingAddressId == null && filterModel.FromDate == null && filterModel.ToDate == null && filterModel.radiobtn == null)
            {
                ds = null;
            }

            Report = new rptrevenue();
            Report.DataSource = ds;
            return Page();

        }
    }
}
