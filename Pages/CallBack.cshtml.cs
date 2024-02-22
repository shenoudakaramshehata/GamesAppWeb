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
using Gameapp.ViewModels;

namespace Gameapp.Pages
{
    public class CallBackModel : PageModel
    {
        private readonly GamesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public List<Order> order { get; set; }
        public ApplicationUser user { set; get; }
        private IHostingEnvironment _env;
        public InvoiceVm invoiceVm { get; set; }
        private readonly IRazorPartialToStringRenderer _renderer;

        private readonly IConfiguration _configuration;
        public string PaymentStatus { get; set; }
        public CallBackModel(IRazorPartialToStringRenderer renderer,GamesContext context, IEmailSender emailSender, UserManager<ApplicationUser> userManager, IHostingEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _env = env;
            _configuration = configuration;
            _renderer = renderer;
        }



        public async Task<ActionResult> OnGet(string tap_id, string data)
        {
            try
            {
                //if (tap_id is null || data is null)
                //{
                //    return RedirectToPage("SomethingwentError", new { Message = "tap_id || data Is Null" });
                //}
                if (tap_id is null)
                {
                    return RedirectToPage("SomethingwentError", new { Message = "tap_id" });
                }

                string Id = tap_id;
                string Data = data;

                var client = new RestClient("https://api.tap.company/v2/charges/" + Id);
                bool TapStatus = bool.Parse(_configuration["TapStatus"]);
                var TestToken = _configuration["TestToken"];
                var LiveToken = _configuration["LiveToken"];
                var request = new RestRequest();
                if (TapStatus)    //Tap Payment Live
                {
                    request.AddHeader("authorization", LiveToken);
                    request.AddParameter("undefined", "{}", ParameterType.RequestBody);
                }
                else
                {
                    request.AddHeader("authorization", TestToken);
                    request.AddParameter("undefined", "{}", ParameterType.RequestBody);
                }
                RestResponse response = client.Execute(request);

                var DeserializedResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

                var Status = DeserializedResponse["status"].ToString();
                PaymentStatus = Status.ToString();

                var MetaData = DeserializedResponse["metadata"];
                var customerEnName = MetaData?["udf2"];
                var customerId = MetaData?["udf1"];

                var Reference = DeserializedResponse["reference"];
                var orderId = Reference?["order"];


                if (Status == "CAPTURED")
                {
                    try
                    {
                        Coupon coupon = null;


                        order = await _context.Order.Where(e => e.uniqeId ==(int) orderId).ToListAsync();

                        foreach (var item in order)
                        {
                            item.ispaid = true;

                            item.PaymentID = Id;

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
                            var shopEmail = _userManager.Users.Where(e => e.EntityId == item.ShopId).FirstOrDefault().Email;
                            //var webRoot = _env.WebRootPath;

                            //var pathToFile = _env.WebRootPath
                            //       + Path.DirectorySeparatorChar.ToString()
                            //       + "Templates"
                            //       + Path.DirectorySeparatorChar.ToString()
                            //       + "EmailTemplate"
                            //       + Path.DirectorySeparatorChar.ToString()
                            //       + "Email.html";
                            //var builder = new BodyBuilder();
                            //using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                            //{

                            //    builder.HtmlBody = SourceReader.ReadToEnd();

                            //}
                            //string messageBody = string.Format(builder.HtmlBody,
                            //   item.Deliverycost,
                            //   item.OrderDiscount,
                            //   item.OrderNet,
                            //   Customer.CustomernameAr,
                            //   item.OrderTotal
                            //   );
                            //await _emailSender.SendEmailAsync(Customer.CustomerEmail, "Order Details", messageBody);

                            //await _emailSender.SendEmailAsync(shopEmail, "Order Details", messageBody);



                            var webRoot = _env.WebRootPath;

                            var pathToFile = _env.WebRootPath
                                   + Path.DirectorySeparatorChar.ToString()
                                   + "Templates"
                                   + Path.DirectorySeparatorChar.ToString()
                                   + "EmailTemplate"
                                   + Path.DirectorySeparatorChar.ToString()
                                   + "Email.html";
                            var builder = new BodyBuilder();
                            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                            {

                                builder.HtmlBody = SourceReader.ReadToEnd();

                            }
                            //string messageBody = string.Format(builder.HtmlBody,
                            //   shoppingCost,
                            //   order.OrderDiscount,
                            //   order.OrderNet,
                            //   Customer.CustomerName,
                            //   order.OrderTotal,
                            //   order.OrderSerial

                            //   );
                            var SocialLinks = _context.SoicialMidiaLink.FirstOrDefault();
                            var SiteContact = _context.ContactUs.FirstOrDefault();
                            invoiceVm = _context.Order.Include(a => a.ShippingAddress).Include(a => a.Customer).Where(e => e.OrderId == item.OrderId).Select(i => new InvoiceVm
                            {
                                OrderId = i.OrderId,
                                OrderDate = i.OrderDate.Date.Year + " / " + i.OrderDate.Date.Month + " / " + i.OrderDate.Date.Day,
                                orderNo = i.uniqeId.ToString(),
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

                            var messageBody = await _renderer.RenderPartialToStringAsync("_Invoice", invoiceVm);
                            await _emailSender.SendEmailAsync(Customer.CustomerEmail, "Order Details", messageBody);

                            await _emailSender.SendEmailAsync(shopEmail, "Order Details", messageBody);
                        }
                        return Page();

                    }
                    catch (Exception ex)
                    {
                        return RedirectToPage("SomethingwentError", new { Message = ex.Message });
                    }


                    
                }
                else
                {
                    try
                    {
                        order = _context.Order.Where(e => e.uniqeId ==(int) orderId).ToList();
                        _context.Order.RemoveRange(order);
                        _context.SaveChanges();
                        return Page(); ;
                    }

                    catch (Exception ex)
                    {
                        return RedirectToPage("SomethingwentError", new { Message = ex.Message });
                    }
                }


            }
            catch (Exception ex )
            {

                return RedirectToPage("SomethingwentError", new { Message = ex.Message });

            }

        }


    }
}
