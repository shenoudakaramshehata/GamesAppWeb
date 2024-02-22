using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.UI.Services;
using Gameapp.Models;
using Gameapp.Data;
using System.IO;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace Gameapp.Pages
{
    public class FattorahErrorModel : PageModel
    {
      
        private GamesContext _context;
        public List<Order> order { get; set; }

        private readonly IEmailSender _emailSender;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        string res { set; get; }
        FattorhResult FattoraResStatus { set; get; }
        private readonly UserManager<ApplicationUser> _userManager;

        public FattorahErrorModel(GamesContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _emailSender = emailSender;
            _configuration = configuration;
            _env = env;
            _userManager = userManager;
        }

        public FattorahPaymentResult fattorahPaymentResult { get; set; }
        static string token = "rLtt6JWvbUHDDhsZnfpAhpYk4dxYDQkbcPTyGaKp2TYqQgG7FGZ5Th_WD53Oq8Ebz6A53njUoo1w3pjU1D4vs_ZMqFiz_j0urb_BH9Oq9VZoKFoJEDAbRZepGcQanImyYrry7Kt6MnMdgfG5jn4HngWoRdKduNNyP4kzcp3mRv7x00ahkm9LAK7ZRieg7k1PDAnBIOG3EyVSJ5kK4WLMvYr7sCwHbHcu4A5WwelxYK0GMJy37bNAarSJDFQsJ2ZvJjvMDmfWwDVFEVe_5tOomfVNt6bOg9mexbGjMrnHBnKnZR1vQbBtQieDlQepzTZMuQrSuKn-t5XZM7V6fCW7oP-uXGX-sMOajeX65JOf6XVpk29DP6ro8WTAflCDANC193yof8-f5_EYY-3hXhJj7RBXmizDpneEQDSaSz5sFk0sV5qPcARJ9zGG73vuGFyenjPPmtDtXtpx35A-BVcOSBYVIWe9kndG3nclfefjKEuZ3m4jL9Gg1h2JBvmXSMYiZtp9MR5I6pvbvylU_PP5xJFSjVTIz7IQSjcVGO41npnwIxRXNRxFOdIUHn0tjQ-7LwvEcTXyPsHXcMD8WtgBh-wxR8aKX7WPSsT1O8d8reb2aR7K3rkV3K82K_0OgawImEpwSvp9MNKynEAJQS6ZHe_J_l77652xwPNxMRTMASk1ZsJL";
        static string baseURL = "https://apitest.myfatoorah.com/v2/GetPaymentStatus";
        static string testURL = "https://apitest.myfatoorah.com/v2/GetPaymentStatus";
        static string liveURL = "https://api.myfatoorah.com/v2/GetPaymentStatus";
        public async Task<IActionResult> OnGet(string paymentId)
        {

            var GetPaymentStatusRequest = new
            {
                Key = paymentId,
                KeyType = "paymentId"
            };
            bool Fattorahstatus = bool.Parse(_configuration["FattorahStatus"]);
            var TestToken = _configuration["TestToken"];
            var LiveToken = _configuration["LiveToken"];
            var GetPaymentStatusRequestJSON = JsonConvert.SerializeObject(GetPaymentStatusRequest);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (Fattorahstatus) // fattorah live
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveToken);
                var httpContent = new StringContent(GetPaymentStatusRequestJSON, System.Text.Encoding.UTF8, "application/json");
                var responseMessage = client.PostAsync(liveURL, httpContent);
                res = await responseMessage.Result.Content.ReadAsStringAsync();
                FattoraResStatus = JsonConvert.DeserializeObject<FattorhResult>(res);
            }
            else                 // fattorah test
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestToken);
                var httpContent = new StringContent(GetPaymentStatusRequestJSON, System.Text.Encoding.UTF8, "application/json");
                var responseMessage = client.PostAsync(testURL, httpContent);
                res = await responseMessage.Result.Content.ReadAsStringAsync();
                FattoraResStatus = JsonConvert.DeserializeObject<FattorhResult>(res);
            }



            if (FattoraResStatus.IsSuccess == true)
            {
                Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
                fattorahPaymentResult = jObject["Data"].ToObject<FattorahPaymentResult>();
                int orderId = 0;
                bool checkRes = int.TryParse(fattorahPaymentResult.UserDefinedField, out orderId);
                if (fattorahPaymentResult.InvoiceStatus == "Paid")
                {
                    try
                    {
                        if (fattorahPaymentResult.UserDefinedField != null)
                        {

                            if (checkRes)
                            {

                                Coupon coupon = null;


                                order = await _context.Order.Where(e => e.uniqeId == orderId).ToListAsync();

                                foreach (var item in order)
                                {
                                    item.ispaid = true;

                                    item.PaymentID = paymentId;

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
                                    string messageBody = string.Format(builder.HtmlBody,
                                       item.Deliverycost,
                                       item.OrderDiscount,
                                       item.OrderNet,
                                       Customer.CustomernameAr,
                                       item.OrderTotal
                                       );
                                    await _emailSender.SendEmailAsync(Customer.CustomerEmail, "Order Details", messageBody);
                                    await _emailSender.SendEmailAsync(shopEmail, "Order Details", messageBody);

                                }


                                return Page();
                            }
                        }
                        return RedirectToPage("SomethingwentError", new { Message = "SomeThing Went Error,try again" });

                    }
                    catch (Exception)
                    {
                        return RedirectToPage("SomethingwentError", new { Message = "Something went wrong" });
                    }


                }
                else
                {
                    try
                    {
                        if (fattorahPaymentResult.UserDefinedField != null)
                        {
                            if (checkRes)
                            {
                                order = _context.Order.Where(e => e.uniqeId == orderId).ToList();
                                _context.Order.RemoveRange(order);
                                _context.SaveChanges();
                                return Page();
                            }
                            return RedirectToPage("SomethingwentError", new { Message = "SomeThing Went Error,try again" });
                        }
                    }

                    catch (Exception)
                    {
                        return RedirectToPage("SomethingwentError", new { Message = "Something went wrong" });
                    }
                }

            }
            return Page();
        }
    }
}
