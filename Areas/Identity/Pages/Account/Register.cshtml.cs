using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gameapp.Data;
using Gameapp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace Gameapp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        private readonly IEmailSender _emailSender;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        private GamesContext _context;
        static string token = "rLtt6JWvbUHDDhsZnfpAhpYk4dxYDQkbcPTyGaKp2TYqQgG7FGZ5Th_WD53Oq8Ebz6A53njUoo1w3pjU1D4vs_ZMqFiz_j0urb_BH9Oq9VZoKFoJEDAbRZepGcQanImyYrry7Kt6MnMdgfG5jn4HngWoRdKduNNyP4kzcp3mRv7x00ahkm9LAK7ZRieg7k1PDAnBIOG3EyVSJ5kK4WLMvYr7sCwHbHcu4A5WwelxYK0GMJy37bNAarSJDFQsJ2ZvJjvMDmfWwDVFEVe_5tOomfVNt6bOg9mexbGjMrnHBnKnZR1vQbBtQieDlQepzTZMuQrSuKn-t5XZM7V6fCW7oP-uXGX-sMOajeX65JOf6XVpk29DP6ro8WTAflCDANC193yof8-f5_EYY-3hXhJj7RBXmizDpneEQDSaSz5sFk0sV5qPcARJ9zGG73vuGFyenjPPmtDtXtpx35A-BVcOSBYVIWe9kndG3nclfefjKEuZ3m4jL9Gg1h2JBvmXSMYiZtp9MR5I6pvbvylU_PP5xJFSjVTIz7IQSjcVGO41npnwIxRXNRxFOdIUHn0tjQ-7LwvEcTXyPsHXcMD8WtgBh-wxR8aKX7WPSsT1O8d8reb2aR7K3rkV3K82K_0OgawImEpwSvp9MNKynEAJQS6ZHe_J_l77652xwPNxMRTMASk1ZsJL";
        public HttpClient httpClient { get; set; }

        public RegisterModel(GamesContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            IWebHostEnvironment host,
            IEmailSender emailSender,
            IConfiguration configuration

            )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _host = host;
            _emailSender = emailSender;
            httpClient = new HttpClient();
            _configuration = configuration;
        }


        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        [BindProperty]
        public Gameapp.Models.ViewModels.AddPublicShop Shop { get; set; }
        
        [BindProperty]
        public int PlanId { get; set; }
        public async Task<ActionResult> OnGetAsync(int id, string returnUrl = null)
        {
            if(id == 0)
            {
                return NotFound();
            }

            PlanId = id;


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile Logo, IFormFile Banner,string returnUrl = null)
        {
            Shop.RegisterDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                
                if(_db.ApplicationUsers.FirstOrDefault(s => s.Email == Shop.Email) != null)
                {
                    ModelState.AddModelError("EmailExist", "Email Already Exist");
                    return Page();
                }

                var user = new ApplicationUser { UserName = Shop.UserName, Email = Shop.Email };
                var result = await _userManager.CreateAsync(user, Shop.Password);

                if (result.Succeeded)
                {

                    Models.Shop model = new Models.Shop()
                    {
                        Address = Shop.Address,
                        Email = Shop.Email,
                        ShopNo = Shop.ShopNo,
                        Password = Shop.Password,
                        Mobile = Shop.Mobile,
                        Deliverycost = Shop.Deliverycost,
                        UserName = Shop.UserName,
                        IsActive = false,
                        ShopTlar = Shop.ShopTlar,
                        ShopTlen = Shop.ShopTlen,
                        Tele = Shop.Tele,
                        RegisterDate = DateTime.Now,
                        OrderIndex = Shop.OrderIndex,
                        CountryId = Shop.CountryId
                    };

                    
                        var uniqeFileNamePic = "";

                    if (Logo !=  null)
                    {


                        string uploadFolder = Path.Combine(_host.WebRootPath, "images");

                        uniqeFileNamePic = Guid.NewGuid() + "_" + Logo.FileName;

                        string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileNamePic);

                        using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                        {
                            Logo.CopyTo(fileStream);
                        }

                    }
                    else
                    {
                        //Create Default Picture
                            DefaultAvatar def = new DefaultAvatar(_host);

                            uniqeFileNamePic = def.CreateProfilePicture(Shop.ShopTlen);
                    }

                 

                    var uniqeFileNameBanner = "";

                    if (Banner != null)
                    {

                        string uploadFolder = Path.Combine(_host.WebRootPath, "images");

                        uniqeFileNameBanner = Guid.NewGuid() + "_" + Banner.FileName;

                        string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileNameBanner);

                        using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                        {
                            Banner.CopyTo(fileStream);
                        }

                    }else
                    {
                        string uploadFolder = Path.Combine(_host.WebRootPath, "images");

                        uniqeFileNameBanner = "bannerdefault.jpeg";

                        string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileNameBanner);
                    }


                    //Create Role
                    await _userManager.AddToRoleAsync(user, "Shop");


                    model.Pic = uniqeFileNamePic;
                    model.Banner = uniqeFileNameBanner;
                    _context.Shop.Add(model);
                    await _context.SaveChangesAsync();

                    //var shop = _db.ApplicationUsers.FirstOrDefault(c => c.Email == Shop.Email);

                    user.EntityId = model.ShopId;
                    user.Pic = uniqeFileNamePic;

                    _db.SaveChanges();
                    var subscription = new Subscriptions()
                    {
                        PlanId = this.PlanId,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(_context.Plans.FindAsync(this.PlanId).Result.Period),
                        ShopId = user.EntityId,
                        Active = false

                    };
                    _context.Subscriptions.Add(subscription);


                    _context.SaveChanges();
                    var PlanCost = _context.Plans.Find(PlanId).Price;

                    bool TapStatus = bool.Parse(_configuration["TapStatus"]);
                    var TestToken = _configuration["TestToken"];
                    var LiveToken = _configuration["LiveToken"];
                    if (TapStatus) // Tap Payment live
                    {
                        var TapMessage = new
                        {
                            amount = PlanCost,
                            currency = "KWD",
                            threeDSecure = true,
                            save_card = false,
                            description = "Games Site Fees",
                            statement_descriptor = "Sample",
                            metadata = new
                            {
                                udf1 = model.ShopId,
                                udf2 = model.ShopTlen,
                            },
                            reference = new
                            {
                                transaction = "txn_0001",
                                order = subscription.Id
                            },
                            receipt = new
                            {
                                email = false,
                                sms = true
                            },
                            customer = new
                            {
                                first_name = model.ShopTlen,
                                middle_name = "test",
                                last_name = "test",
                                email = model.Email,
                                phone = new
                                {
                                    country_code = "965",
                                    //number = "50143413"
                                    number = "50000000"
                                }
                            },
                            //merchant = new { id = "194729" },
                            merchant = new { id = "" },
                            source = new { id = "src_kw.knet" },
                            redirect = new { url = "https://thegameskw.com/RegisterShopCallBack" }
                            //redirect = new { url = "https://localhost:44360/RegisterShopCallBack" }
                        };

                        var sendPaymentRequestJSON = JsonConvert.SerializeObject(TapMessage);

                        var client = new RestClient("https://api.tap.company/v2/charges");
                        var request = new RestRequest();
                        request.AddHeader("content-type", "application/json");
                        request.AddHeader("authorization", LiveToken);
                        request.AddParameter("application/json", sendPaymentRequestJSON, ParameterType.RequestBody);
                        RestResponse response = await client.PostAsync(request);

                        var DeserializeObjectResopnse = JsonConvert.DeserializeObject<JObject>(response.Content);

                        var Transaction = DeserializeObjectResopnse.GetValue("transaction");

                        var Url = Transaction["url"].ToString();
                        return Redirect(Url);

                    }
                    else               // Tap Payment test
                    {
                        var TapMessage = new
                        {
                            amount = PlanCost,
                            currency = "KWD",
                            threeDSecure = true,
                            save_card = false,
                            description = "Games Site Fees",
                            statement_descriptor = "Sample",
                            metadata = new
                            {
                                udf1 = model.ShopId,
                                udf2 = model.ShopTlen,
                            },
                            reference = new
                            {
                                transaction = "txn_0001",
                                order = subscription.Id
                            },
                            receipt = new
                            {
                                email = false,
                                sms = true
                            },
                            customer = new
                            {
                                first_name = model.ShopTlen,
                                middle_name = "test",
                                last_name = "test",
                                email = model.Email,
                                phone = new
                                {
                                    country_code = "965",
                                    //number = "50143413"
                                    number = "50000000"
                                }
                            },
                            merchant = new { id = "" },
                            source = new { id = "src_kw.knet" },
                            redirect = new { url = "https://thegameskw.com/RegisterShopCallBack" }
                            //redirect = new { url = "https://localhost:44360/RegisterShopCallBack" }
                        };

                        var sendPaymentRequestJSON = JsonConvert.SerializeObject(TapMessage);

                        var client = new RestClient("https://api.tap.company/v2/charges");
                        var request = new RestRequest();
                        request.AddHeader("content-type", "application/json");
                        request.AddHeader("authorization", TestToken);
                        request.AddParameter("application/json", sendPaymentRequestJSON, ParameterType.RequestBody);
                        RestResponse response = await client.PostAsync(request);

                        var DeserializeObjectResopnse = JsonConvert.DeserializeObject<JObject>(response.Content);

                        var Transaction = DeserializeObjectResopnse.GetValue("transaction");

                        var Url = Transaction["url"].ToString();
                        return Redirect(Url);


                    }


                    //bool Fattorahstatus = bool.Parse(_configuration["FattorahStatus"]);
                    //var TestToken = _configuration["TestToken"];
                    //var LiveToken = _configuration["LiveToken"];
                    //if (Fattorahstatus) // fattorah live
                    //{
                    //    var sendPaymentRequest = new
                    //    {


                    //        CustomerName = model.ShopTlen,
                    //        NotificationOption = "LNK",
                    //        InvoiceValue = PlanCost,
                    //        CallBackUrl = "http://thegameskw.com/FattorahShopSuccess",
                    //        ErrorUrl = "http://thegameskw.com/FattorahShopError",
                    //        UserDefinedField = subscription.Id,
                    //        CustomerEmail = model.Email

                    //    };
                    //    var sendPaymentRequestJSON = JsonConvert.SerializeObject(sendPaymentRequest);

                    //    string url = "https://apitest.myfatoorah.com/v2/SendPayment";
                    //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveToken);
                    //    var httpContent = new StringContent(sendPaymentRequestJSON, Encoding.UTF8, "application/json");
                    //    var responseMessage = httpClient.PostAsync(url, httpContent);
                    //    var res = await responseMessage.Result.Content.ReadAsStringAsync();
                    //    var FattoraRes = JsonConvert.DeserializeObject<FattorhResult>(res);


                    //    if (FattoraRes.IsSuccess == true)
                    //    {
                    //        Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
                    //        var InvoiceRes = jObject["Data"].ToObject<InvoiceData>();
                    //        return Redirect(InvoiceRes.InvoiceURL);

                    //    }
                    //    else
                    //    {
                    //        return RedirectToPage("SomethingwentError", new { Message = FattoraRes.Message });
                    //    }

                    //}
                    //else               // fattorah test
                    //{
                    //    var sendPaymentRequest = new
                    //    {


                    //        CustomerName = model.ShopTlen,
                    //        NotificationOption = "LNK",
                    //        InvoiceValue = PlanCost,
                    //        CallBackUrl = "http://thegameskw.com/FattorahShopSuccess",
                    //        ErrorUrl = "http://thegameskw.com/FattorahShopError",
                    //        UserDefinedField = subscription.Id,
                    //        CustomerEmail = model.Email

                    //    };
                    //    var sendPaymentRequestJSON = JsonConvert.SerializeObject(sendPaymentRequest);

                    //    string url = "https://apitest.myfatoorah.com/v2/SendPayment";
                    //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestToken);
                    //    var httpContent = new StringContent(sendPaymentRequestJSON, Encoding.UTF8, "application/json");
                    //    var responseMessage = httpClient.PostAsync(url, httpContent);
                    //    var res = await responseMessage.Result.Content.ReadAsStringAsync();
                    //    var FattoraRes = JsonConvert.DeserializeObject<FattorhResult>(res);


                    //    if (FattoraRes.IsSuccess == true)
                    //    {
                    //        Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
                    //        var InvoiceRes = jObject["Data"].ToObject<InvoiceData>();
                    //        return Redirect(InvoiceRes.InvoiceURL);

                    //    }
                    //    else
                    //    {
                    //        return RedirectToPage("SomethingwentError", new { Message = FattoraRes.Message });
                    //    }

                    //}


                    string html =
@"
<div style='display: table;margin:auto; border: 1px solid #ccc;'>
<tbody style='
    text-align: center;
    margin: auto;
' ><tr>
                      <td>
                        
                        <table style='margin:auto' align='center' border='0' bgcolor='#ffffff' class='m_-3593417453714472844mlContentTable' cellpadding='0' cellspacing='0' width='640'>
                          <tbody  style='margin:auto'><tr>
                            <td>
                              <table align='center' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='m_-3593417453714472844mlContentTable' style='width:640px;min-width:640px' width='640'>
                                <tbody ><tr>
                                  <td>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='40' class='m_-3593417453714472844spacingHeight-40' style='line-height:40px;min-height:40px'></td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td align='center' style='padding:0px 40px' class='m_-3593417453714472844mlContentOuter'>
                                          <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='100%'>
                                            <tbody><tr>
                                              <td align='center'>
                                                <a href='http://codewarenet-001-site2.dtempurl.com/'>
<img src='https://g.top4top.io/p_2210qvkhz1.png' id='m_-3593417453714472844logoBlock-4' border='0' alt='' width='246' style='display:block' class='CToWUd'>
</a>
                                              </td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='10' style='line-height:10px;min-height:10px'></td>
                                      </tr>
                                    </tbody></table>
                                  </td>
                                </tr>
                              </tbody></table>
                            </td>
                          </tr>
                        </tbody></table>
                        
                        
                        <table align='center' border='0' bgcolor='#ffffff' class='m_-3593417453714472844mlContentTable' cellpadding='0' cellspacing='0' width='640'>
                          <tbody><tr>
                            <td>
                              <table align='center' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='m_-3593417453714472844mlContentTable' style='width:640px;min-width:640px' width='640'>
                                <tbody><tr>
                                  <td>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='20' class='m_-3593417453714472844spacingHeight-20' style='line-height:20px;min-height:20px'></td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td align='center' style='padding:0px 40px' class='m_-3593417453714472844mlContentOuter'>
                                          <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='100%'>
                                            <tbody><tr>
                                              <td id='m_-3593417453714472844bodyText-6' style='font-family:'Inter',sans-serif;font-size:15px;line-height:150%;color:#000'>
                                                <p style='margin-top:0px;margin-bottom:10px;line-height:150%'>Hey there!</p>
                                                <p style='margin-top:0px;margin-bottom:10px;line-height:150%'>We're <strong>extremely </strong>proud to let you know that we've just released  a <strong>brand new</strong> update to our bestselling beginner's PHP book: <a href='https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0MDkmZD1rNWIzZjZ5.0gsFAHlQ2wiLQX9hfRptWdDnKidAzGFt3PlWw5uFsnk' style='word-break:break-word;font-family:'Inter',sans-serif;color:#7232fa;text-decoration:underline' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0MDkmZD1rNWIzZjZ5.0gsFAHlQ2wiLQX9hfRptWdDnKidAzGFt3PlWw5uFsnk&amp;source=gmail&amp;ust=1642694584579000&amp;usg=AOvVaw0XMpaQk1mgtlBOMVyoVfbh'><strong>PHP &amp; MySQL: Novice to Ninja</strong></a><em>.</em> Now in its seventh edition, we've worked with expert author Tom&nbsp;Butler to thoroughly revise the content to cover PHP 8.1, the latest version of the popular open-source web development scripting language . In addition, we've moved to a Docker-based setup to make installation of PHP and the setup of the plethora of&nbsp;examples shown in the book a
snap.<br></p>
                                                <p style='margin-top:0px;margin-bottom:10px;line-height:150%'><em><a href='https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0MjEmZD10OWkzZjln.64kKb8v-2vPyPd4E1rufVfBAlkdo2Eje-OOa13e2yQU' style='word-break:break-word;font-family:'Inter',sans-serif;color:#7232fa;text-decoration:underline' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0MjEmZD10OWkzZjln.64kKb8v-2vPyPd4E1rufVfBAlkdo2Eje-OOa13e2yQU&amp;source=gmail&amp;ust=1642694584579000&amp;usg=AOvVaw0d6cxbPjRYLdNfKJ-FOGnc'>PHP &amp; MySQL: Novice to Ninja</a></em> remains the <strong>easiest</strong> and <strong>best</strong> way to learn PHP. It doesn't assume any prior programming experience.</p>
                                                <ul style='margin-top:0px;margin-bottom:10px'></ul>
                                              </td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                  
                                                  </td>
                                                </tr>
                                              </tbody></table>
                                              </td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='20' class='m_-3593417453714472844spacingHeight-20' style='line-height:20px;min-height:20px'></td>
                                      </tr>
                                    </tbody></table>
                                  </td>
                                </tr>
                              </tbody></table>
                            </td>
                          </tr>
                        </tbody></table>
                        
                        
                        <table align='center' border='0' bgcolor='#ffffff' class='m_-3593417453714472844mlContentTable' cellpadding='0' cellspacing='0' width='640'>
                          <tbody><tr>
                            <td>
                              <table align='center' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='m_-3593417453714472844mlContentTable' style='width:640px;min-width:640px' width='640'>
                                <tbody><tr>
                                  <td>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='20' class='m_-3593417453714472844spacingHeight-20' style='line-height:20px;min-height:20px'></td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td align='center' style='padding:0px 40px' class='m_-3593417453714472844mlContentOuter'>
                                          <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='100%'>
                                            <tbody><tr>
                                              <td id='m_-3593417453714472844bodyText-10' style='font-family:'Inter',sans-serif;font-size:15px;line-height:150%;color:#000'>
                                                <p style='margin-top:0px;margin-bottom:10px;line-height:150%'>PHP is used on a staggering 80% of the top 10 million websites in the world. Start your jouney to becoming a PHP ninja by reading this book.<br></p>
                                                <p style='margin-top:0px;margin-bottom:10px;line-height:150%'>Happy learning!<br></p>
                                                <p style='margin-top:0px;margin-bottom:0px;line-height:150%'>Dianne&nbsp;from SitePoint</p>
                                              </td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='20' class='m_-3593417453714472844spacingHeight-20' style='line-height:20px;min-height:20px'></td>
                                      </tr>
                                    </tbody></table>
                                  </td>
                                </tr>
                              </tbody></table>
                            </td>
                          </tr>
                        </tbody></table>
                        
                        
                        <table align='center' border='0' bgcolor='#ffffff' class='m_-3593417453714472844mlContentTable' cellpadding='0' cellspacing='0' width='640'>
                          <tbody><tr>
                            <td>
                              <table align='center' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='m_-3593417453714472844mlContentTable' style='width:640px;min-width:640px' width='640'>
                                <tbody><tr>
                                  <td>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='20' class='m_-3593417453714472844spacingHeight-20' style='line-height:20px;min-height:20px'></td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td align='center'>
                                          <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='100%' style='border-top:1px solid #ededf3;border-collapse:initial'>
                                            <tbody><tr>
                                              <td height='20' class='m_-3593417453714472844spacingHeight-20' style='line-height:20px;min-height:20px'></td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                  </td>
                                </tr>
                              </tbody></table>
                            </td>
                          </tr>
                        </tbody></table>
                        
                        <table align='center' border='0' bgcolor='#ffffff' class='m_-3593417453714472844mlContentTable' cellpadding='0' cellspacing='0' width='640'>
                          <tbody><tr>
                            <td>
                              <table align='center' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='m_-3593417453714472844mlContentTable' style='width:640px;min-width:640px' width='640'>
                                <tbody><tr>
                                  <td>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='30' class='m_-3593417453714472844spacingHeight-30' style='line-height:30px;min-height:30px'></td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td align='center' style='padding:0px 40px' class='m_-3593417453714472844mlContentOuter'>
                                          <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='100%'>
                                            <tbody><tr>
                                              <td align='left' style='font-family:'Inter',sans-serif;font-size:14px;font-weight:700;line-height:150%;color:#111111'>SitePoint</td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='10'></td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td align='center' style='padding:0px 40px' class='m_-3593417453714472844mlContentOuter'>
                                          <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='100%'>
                                            <tbody><tr>
                                              <td align='center'>
                                                <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='left' width='267' style='width:267px;min-width:267px' class='m_-3593417453714472844mlContentTable m_-3593417453714472844marginBottom'>
                                                  <tbody><tr>
                                                    <td align='left' id='m_-3593417453714472844footerText-14' style='font-family:'Inter',sans-serif;font-size:12px;line-height:150%;color:#111111'>
                                                      <p style='margin-top:0px;margin-bottom:0px'>10-20 Gwynne Street, Cremorne<br>Australia</p>
                                                    </td>
                                                  </tr>
                                                  <tr>
                                                    <td height='25' class='m_-3593417453714472844spacingHeight-20'></td>
                                                  </tr>
                                                  <tr>
                                                    <td align='center'>
                                                      <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='left'>
                                                        <tbody><tr>
                                                          <td align='center' width='24' style='padding:0px 5px'>
                                                            <a href='https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0NDgmZD1tNG85ZDZ2.ETNSWAj32nzLlydWkAjQxVhHIFre6SHCvFF93MonZ4I' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0NDgmZD1tNG85ZDZ2.ETNSWAj32nzLlydWkAjQxVhHIFre6SHCvFF93MonZ4I&amp;source=gmail&amp;ust=1642694584579000&amp;usg=AOvVaw1HrySKKiwcwPsZDlOL2-gc'>
<img width='24' alt='discord' src='https://ci3.googleusercontent.com/proxy/I804m_5Cgy6ErPHkf0_8I1vlioZocc2bjML7BHQBtxqxS9uT3OOcdgss2v-DMG0nf9goACa2EDbFFPcjl610ykmMn2IIvUvYs68xa7D1Cu_w8EPgY5mU49SX=s0-d-e1-ft#https://cdn.mailerlite.com/images/icons/default/round/black/discord.png' style='display:block' border='0' class='CToWUd'>
</a>
                                                          </td>
                                                          <td align='center' width='24' style='padding:0px 5px'>
                                                            <a href='https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0NTQmZD1pMms0aDZo.FknUpJ2PcPjYofuO2y4dyRPlHzXYVeGMjBdbnMJHEPQ' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0NTQmZD1pMms0aDZo.FknUpJ2PcPjYofuO2y4dyRPlHzXYVeGMjBdbnMJHEPQ&amp;source=gmail&amp;ust=1642694584579000&amp;usg=AOvVaw24J8BNeSO7DXRHqf10hSKo'>
<img width='24' alt='twitter' src='https://ci5.googleusercontent.com/proxy/8-p7u4CE9SAlowSzP8fzBYe1zWlja-iVGe_nvhIIizFQE-u5jGgSafruMS6eBLyFJYVsijDZfpryFAjkcXARz-X2KIqYTM_PAuGiguHRu-luZAORo75QDZ0s=s0-d-e1-ft#https://cdn.mailerlite.com/images/icons/default/round/black/twitter.png' style='display:block' border='0' class='CToWUd'>
</a>
                                                          </td>
                                                          <td align='center' width='24' style='padding:0px 5px'>
                                                            <a href='https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0NTcmZD1kNmY4ZDNn.pPnurbodoHQEajzISfcFCD1KNMAAnL7Oflz-Rk252jA' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzE0NTcmZD1kNmY4ZDNn.pPnurbodoHQEajzISfcFCD1KNMAAnL7Oflz-Rk252jA&amp;source=gmail&amp;ust=1642694584579000&amp;usg=AOvVaw1c2iDfnKHU7IFk4qKNMzCN'>
<img width='24' alt='facebook' src='https://ci6.googleusercontent.com/proxy/rbCPdi0uPTL_Ax_mjpL2419JKMEVYlYNR7_OsiCVveoOIJkQogebSzU3wScRtQiQuXWPa5p-6NY4xa6EstDQqWhDImuFQuKCFrZ1WcqLb_yTYZQNLSfMUtfJ8Q=s0-d-e1-ft#https://cdn.mailerlite.com/images/icons/default/round/black/facebook.png' style='display:block' border='0' class='CToWUd'>
</a>
                                                          </td>
                                                        </tr>
                                                      </tbody></table>
                                                    </td>
                                                  </tr>
                                                </tbody></table>
                                                <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='right' width='267' style='width:267px;min-width:267px' class='m_-3593417453714472844mlContentTable'>
                                                  <tbody><tr>
                                                    <td align='right' id='m_-3593417453714472844footerUnsubscribeText-14' style='font-family:'Inter',sans-serif;font-size:12px;line-height:150%;color:#111111'>
                                                      <p style='margin-top:0px;margin-bottom:0px'>You received this email because you signed up on our website or made purchase from us.</p>
                                                    </td>
                                                  </tr>
                                                  <tr>
                                                    <td height='10'></td>
                                                  </tr>
                                                  <tr>
                                                    <td align='right' style='font-family:'Inter',sans-serif;font-size:12px;line-height:150%;color:#111111'>
                                                      <a href='https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzI5OTAmZD1nNmIycTll.A4Jxh6myizJ2YhjeH9wtmyalZcTPgcPCtdlhVFMVCgQ' style='color:#111111;text-decoration:underline' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://ml.sitepoint.com/link/c/YT0xODY2MDkzMTM2NDc2OTAzODU3JmM9bTlvMCZlPTAmYj04NzMwMzI5OTAmZD1nNmIycTll.A4Jxh6myizJ2YhjeH9wtmyalZcTPgcPCtdlhVFMVCgQ&amp;source=gmail&amp;ust=1642694584579000&amp;usg=AOvVaw0BsXWkeIm7fynqkV7s_C_m'>
<span style='color:#111111'>Unsubscribe</span>
</a>
                                                    </td>
                                                  </tr>
                                                </tbody></table>
                                              </td>
                                            </tr>
                                          </tbody></table>
                                        </td>
                                      </tr>
                                    </tbody></table>
                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' align='center' width='640' style='width:640px;min-width:640px' class='m_-3593417453714472844mlContentTable'>
                                      <tbody><tr>
                                        <td height='40' class='m_-3593417453714472844spacingHeight-40' style='line-height:40px;min-height:40px'></td>
                                      </tr>
                                    </tbody></table>
                                  </td>
                                </tr>
                              </tbody></table>
                            </td>
                          </tr>
                        </tbody></table>
                      </td>
                    </tr>
                  </tbody>
</div>";

                    await _emailSender.SendEmailAsync(model.Email, "Welcome To Games Store",
                     html);


                    return Redirect("/identity/account/login");
                }

                ModelState.AddModelError("CreateUserError", result.Errors.First().Description);
            }

            return Page();

        }
    }
}
