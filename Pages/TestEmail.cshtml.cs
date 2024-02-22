using Gameapp.Data;
using Gameapp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IO;
using MimeKit;
using Gameapp.ViewModels;
using Gameapp.Services;

namespace Gameapp.Pages
{
    public class TestEmailModel : PageModel
    {
        private readonly GamesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public InvoiceVm invoiceVm { get; set; }
        public TestEmailModel(GamesContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            
        }
        public async Task<IActionResult> OnGet()
        {
            var order = await _context.Order.Where(e => e.uniqeId ==55).ToListAsync();

            foreach (var item in order)
            {

                var Customer = _context.Customer.Find(item.CustomerId);
               
                var shopEmail = _userManager.Users.Where(e => e.EntityId == item.ShopId).FirstOrDefault().Email;
                var SocialLinks = _context.SoicialMidiaLink.FirstOrDefault();
                var SiteContact = _context.ContactUs.FirstOrDefault();
                invoiceVm = _context.Order.Include(a => a.ShippingAddress).Include(a => a.Customer).Where(e => e.OrderId == item.OrderId).Select(i => new InvoiceVm
                {
                    OrderId = i.OrderId,
                    OrderDate = i.OrderDate.Date.Year + " / " + i.OrderDate.Date.Month + " / " + i.OrderDate.Date.Day,
                    orderNo=i.uniqeId.ToString(),
                    OrderTime = i.OrderDate.TimeOfDay.Hours + " : " + i.OrderDate.TimeOfDay.Minutes,
                    Country = _context.Country.FirstOrDefault().CountryTlen,
                    CusName = _context.Customer.Where(e => e.CustomerId == i.CustomerId).FirstOrDefault().CustomernameEn,
                    CusAddress = i.ShippingAddress.AddressNickname + " , " + i.ShippingAddress.Area + " , " + i.ShippingAddress.Street + " , " + i.ShippingAddress.Block + " , " + i.ShippingAddress.Avenue != null ? i.ShippingAddress.Avenue! : "" + " , " + i.ShippingAddress.Building + " , " + i.ShippingAddress.Floor,
                    CusArea = i.ShippingAddress.Area,
                    CusStreet = i.ShippingAddress.Street,
                    CusBuilding = i.ShippingAddress.Block,
                    CusFloor = i.ShippingAddress.Floor,
                    NetOrder = i.OrderNet.Value,
                    OrderTotal = i.OrderTotal,
                    Discount = i.OrderDiscount,
                    InvoiceNumber = i.uniqeId,
                    WebSite = $"{this.Request.Scheme}://{this.Request.Host}",
                    SuppEmail = _context.ContactUs.FirstOrDefault().Email,
                    ConntactNumber = _context.ContactUs.FirstOrDefault().phonenumber1,
                    ShippingCost = _context.Shop.FirstOrDefault(s => s.ShopId == item.ShopId).Deliverycost.Value,
                    ShippingAddress = i.ShippingAddress.AddressNickname + " , " + i.ShippingAddress.Area + " , " + i.ShippingAddress.Street + " , " + i.ShippingAddress.Block + " , " + i.ShippingAddress.Avenue != null ? i.ShippingAddress.Avenue! : "" + " , " + i.ShippingAddress.Building + " , " + i.ShippingAddress.Floor,
                    Address = i.Customer.CustomerAddress,
                    ShippingAddressPhone = i.Customer.Customerphone,
                    PaymentTit = _context.PaymentMehod.Where(e => e.PaymentMethodId == i.PaymentMehodPaymentMethodId).FirstOrDefault().PaymentMethodName,
                    currencyName = "KD",
                    TwitterLink = SocialLinks.TwitterLink,
                    WhatsApplink = SocialLinks.WhatsApplink,
                    Instgramlink = SocialLinks.Instgramlink,
                    SiteEmail = SiteContact.Email,
                    SitePhone = SiteContact.phonenumber1,
                    OrderDateWithAnotherFo = i.OrderDate.ToString("dddd, dd MMMM yyyy"),

                }).FirstOrDefault();
                if (invoiceVm == null)
                {
                    return RedirectToPage("SomethingwentError", new { Message = "invoiceVm Is Null" });
                }
                else
                {
                    var orderItemVm = _context.OrderItems.Include(e => e.Item).Where(e => e.OrderId == invoiceVm.OrderId).Select(i => new OrderItemVm
                    {
                        ItemImage = i.Item.ItemImage,
                        ItemPrice = i.ItemPrice,
                        ItemQuantity = i.ItemQuantity,
                        ItemTitleEn = i.Item.ItemName,
                        Total = i.Total,
                        ItemDis = i.Item.ItemDescription
                    }).ToList();
                    invoiceVm.orderItemVms = orderItemVm;
                }

               
            }
            return Page();

        }
    }
}
