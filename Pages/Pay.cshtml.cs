using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Gameapp.Data;
using Gameapp.Models;
using Gameapp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace Gameapp.Pages
{
    public class PayModel : PageModel
    {
        private GamesContext _context;
        public Order order { get; set; }
        public Models.ViewModels.OrderModel ordersmodel { get; set; }
        static string token = "rLtt6JWvbUHDDhsZnfpAhpYk4dxYDQkbcPTyGaKp2TYqQgG7FGZ5Th_WD53Oq8Ebz6A53njUoo1w3pjU1D4vs_ZMqFiz_j0urb_BH9Oq9VZoKFoJEDAbRZepGcQanImyYrry7Kt6MnMdgfG5jn4HngWoRdKduNNyP4kzcp3mRv7x00ahkm9LAK7ZRieg7k1PDAnBIOG3EyVSJ5kK4WLMvYr7sCwHbHcu4A5WwelxYK0GMJy37bNAarSJDFQsJ2ZvJjvMDmfWwDVFEVe_5tOomfVNt6bOg9mexbGjMrnHBnKnZR1vQbBtQieDlQepzTZMuQrSuKn-t5XZM7V6fCW7oP-uXGX-sMOajeX65JOf6XVpk29DP6ro8WTAflCDANC193yof8-f5_EYY-3hXhJj7RBXmizDpneEQDSaSz5sFk0sV5qPcARJ9zGG73vuGFyenjPPmtDtXtpx35A-BVcOSBYVIWe9kndG3nclfefjKEuZ3m4jL9Gg1h2JBvmXSMYiZtp9MR5I6pvbvylU_PP5xJFSjVTIz7IQSjcVGO41npnwIxRXNRxFOdIUHn0tjQ-7LwvEcTXyPsHXcMD8WtgBh-wxR8aKX7WPSsT1O8d8reb2aR7K3rkV3K82K_0OgawImEpwSvp9MNKynEAJQS6ZHe_J_l77652xwPNxMRTMASk1ZsJL";

        public List<OrderItem> orderItem { get; set; }
        public PaymentMehod paymentMethod { get; set; }
        private readonly IEmailSender _emailSender;
        public HttpClient httpClient { get; set; }

        public PayModel(GamesContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
            httpClient = new HttpClient();
        }
       
        public async Task<IActionResult> OnGet(string Values)
        {
            if (Values != null)
            {

                var checkOutViewModel = JsonConvert.DeserializeObject<Models.ViewModels.CheckOutViewModel>(Values);
                var CustomerObj = _context.Customer.Where(e => e.CustomerId == checkOutViewModel.CustomerAddressId).FirstOrDefault();

                if (CustomerObj == null)
                {
                    return RedirectToPage("SomethingwentError");

                }

                var CustomerAddressObj = _context.ShippingAddress.Where(e => e.ShippingAddressId == checkOutViewModel.CustomerAddressId).FirstOrDefault();

                if (CustomerAddressObj == null)
                {
                    return RedirectToPage("SomethingwentError");


                }
                var paymentMethod = _context.PaymentMehod.Where(e => e.PaymentMethodId == checkOutViewModel.PaymentMehodPaymentMethodId).FirstOrDefault();

                if (checkOutViewModel.PaymentMehodPaymentMethodId == 0)
                {
                    return RedirectToPage("SomethingwentError");
                }
                
                
                float discount = 0.0f;


                //Get Customer ShoppingCart Items List
                var customerShoppingCartList = _context.
                    ShoppingCart.Include(s => s.Customer)
                    .Include(s => s.Item)
                    .ThenInclude(s => s.Shop)
                    .Where(c => c.CustomerId == checkOutViewModel.CustomerId);

                var totalOfAll = customerShoppingCartList.AsEnumerable().Sum(c => c.ItemTotal);

                // make coupon used

                Coupon coupon = null;
                coupon = _context.Coupon.FirstOrDefault(c => c.Id == checkOutViewModel.CouponId);


                //calc ordernet
                float calcOrderNet(float sumItemTotal)
                {
                    var percent = sumItemTotal / totalOfAll;

                    if (coupon == null)
                    {
                        discount = 0.0f;
                        return sumItemTotal;
                    }
                    else if (coupon.CouponTypeId == 2)
                    {
                        discount = sumItemTotal - (float)(sumItemTotal - coupon.Amount * percent);

                        return (float)(sumItemTotal - coupon.Amount * percent);
                    }
                    else
                    {
                        var couponAmount = totalOfAll * (coupon.Amount / 100);
                        discount = sumItemTotal - (float)(sumItemTotal - couponAmount * percent);

                        return (float)(sumItemTotal - couponAmount * percent);
                    }

                }

                int maxUniqe = 1;
                var newList = _context.Order.ToList();
                if (newList.Count >0)
                {
                    maxUniqe= newList.Max(e => e.uniqeId);
                }
                    

                var orders = customerShoppingCartList.AsEnumerable().GroupBy(c => c.Item.ShopId).

                Select(g => new Order
                {
                    OrderDate = DateTime.Now,
                    OrderSerial = Guid.NewGuid().ToString() + "/" + DateTime.Now.Year,
                    ShopId = g.Key.Value,
                    CustomerId = checkOutViewModel.CustomerId,
                    ShippingAddressId = checkOutViewModel.CustomerAddressId,
                    OrderTotal = g.Sum(c => c.ItemTotal),
                    CouponId = coupon != null ? checkOutViewModel.CouponId : null,
                    CouponTypeId = coupon != null ? coupon.CouponTypeId : null,
                    CouponAmount = coupon != null ? (float?)coupon.Amount : null,
                    Deliverycost = _context.Shop.FirstOrDefault(s => s.ShopId == g.Key.Value).Deliverycost,
                    OrderNet = calcOrderNet(g.Sum(c => c.ItemTotal)) + _context.Shop.FirstOrDefault(s => s.ShopId == g.Key.Value).Deliverycost,
                    PaymentMehodPaymentMethodId = checkOutViewModel.PaymentMehodPaymentMethodId,
                    OrderDiscount = discount,
                    uniqeId = maxUniqe + 1

                }).ToList();
                
                
                try
                {
                    foreach (var item in orders)
                    {
                        _context.Order.Add(item);
                        _context.SaveChanges();

                        //transfer shoppingcart to orderitems table and clear shoppingcart

                        List<OrderItem> orderItems = new List<OrderItem>();


                        foreach (var itemshop in customerShoppingCartList)
                        {
                            if (itemshop.Item.ShopId == item.ShopId)
                            {
                                OrderItem orderItem = new OrderItem
                                {
                                    ItemId = (int)itemshop.ItemId,
                                    ItemPrice = itemshop.ItemPrice,
                                    Total = itemshop.ItemTotal,
                                    ItemQuantity = itemshop.ItemQty,
                                    OrderId = item.OrderId
                                };

                                _context.OrderItems.Add(orderItem);

                            }
                        }

                    }
                    _context.SaveChanges();


                }
                catch (Exception)
                {
                    return RedirectToPage("SomethingwentError");
                }
                
               
                
                var Customer = _context.Customer.Find(checkOutViewModel.CustomerId);
                if (Customer == null)
                {
                    return RedirectToPage("SomethingwentError");
                }
                
                if (checkOutViewModel.PaymentMehodPaymentMethodId == 2)
                {
                    var requesturl = "https://api.upayments.com/test-payment";

                    var fields = new
                    {
                        merchant_id = "1201",
                        username = "test",
                        password = "test",
                        order_id = orders.FirstOrDefault().uniqeId,
                        total_price = orders.Sum(e=>e.OrderNet),
                        test_mode = 0,
                        CstFName = Customer.CustomernameEn,
                        CstEmail = Customer.CustomerEmail,
                        CstMobile = Customer.Customerphone,
                        api_key = "jtest123",
                        success_url = "http://techetime-001-site4.atempurl.com/success",
                        error_url = "http://techetime-001-site4.atempurl.com/failed"

                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                    var task = httpClient.PostAsync(requesturl, content);
                    var result = await task.Result.Content.ReadAsStringAsync();
                    var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
                    if (paymenturl.status == "success")

                    {
                        return Redirect(paymenturl.paymentURL);
                    }
                    else
                    {
                        
                        _context.Order.RemoveRange(orders);
                        _context.SaveChanges();
                        return RedirectToPage("SomethingwentError", new { Message = paymenturl.error_msg });

                    }
                }
                else if (checkOutViewModel.PaymentMehodPaymentMethodId == 3)
                {
                   

                    var sendPaymentRequest = new
                    {

                        CustomerName = Customer.CustomernameEn,
                        NotificationOption = "LNK",
                        InvoiceValue = orders.Sum(e => e.OrderNet),
                        CallBackUrl = "http://techetime-001-site4.atempurl.com/FattorahSuccess",
                        ErrorUrl = "http://techetime-001-site4.atempurl.com/FattorahError",
                        UserDefinedField = orders.FirstOrDefault().uniqeId,
                        CustomerEmail = Customer.CustomerEmail

                    };
                    var sendPaymentRequestJSON = JsonConvert.SerializeObject(sendPaymentRequest);

                    string url = "https://apitest.myfatoorah.com/v2/SendPayment";
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var httpContent = new StringContent(sendPaymentRequestJSON, Encoding.UTF8, "application/json");
                    var responseMessage = httpClient.PostAsync(url, httpContent);
                    var res = await responseMessage.Result.Content.ReadAsStringAsync();
                    var FattoraRes = JsonConvert.DeserializeObject<FattorhResult>(res);


                    if (FattoraRes.IsSuccess == true)
                    {
                        Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
                        var InvoiceRes = jObject["Data"].ToObject<InvoiceData>();
                        return Redirect(InvoiceRes.InvoiceURL);

                    }
                    else
                    {
                        return RedirectToPage("SomethingwentError", new { Message = FattoraRes.Message });
                    }

                }









                if (checkOutViewModel.PaymentMehodPaymentMethodId == 1)
                {
                   
                    if (orders.FirstOrDefault().CustomerId != null)
                    {
                        if (coupon != null)
                        {
                            coupon.Used = true;
                            var UpdatedCoupon = _context.Coupon.Attach(coupon);
                            UpdatedCoupon.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        }
                        
                        _context.ShoppingCart.RemoveRange(customerShoppingCartList);
                        _context.SaveChanges();
                        return RedirectToPage("Thankyou");
                        
                    }

                }

            }
            return RedirectToPage("SomethingwentError");
        }
    }
          
}



