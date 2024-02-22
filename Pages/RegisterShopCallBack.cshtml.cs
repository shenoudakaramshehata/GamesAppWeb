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
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IO;
using MimeKit;


namespace Gameapp.Pages
{
    public class RegisterShopCallBackModel : PageModel
    {

        private readonly GamesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public Shop shop { set; get; }
        public ApplicationUser user { set; get; }
        private IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        public string PaymentStatus { get; set; }
        public RegisterShopCallBackModel(GamesContext context, IEmailSender emailSender, UserManager<ApplicationUser> userManager, IHostingEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _env = env;
            _configuration = configuration;
        }
        public async Task<ActionResult> OnGet(string tap_id, string data)
        {
            try
            {
                if (tap_id is null || data is null)
                {
                    return RedirectToPage("SomethingwentError");
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
                        var SubObj = await _context.Subscriptions.Where(e => e.Id == (int)orderId).FirstOrDefaultAsync();
                        SubObj.Active = true;

                        var UpdatedShop = _context.Subscriptions.Attach(SubObj);
                        UpdatedShop.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                        _context.SaveChanges();

                        return Page();
                    }
                    catch (Exception)
                    {
                        return RedirectToPage("SomethingwentError");
                    }



                }
                else
                {
                    try
                    {
                        var SubObj = await _context.Subscriptions.Where(e => e.Id ==(int)orderId).FirstOrDefaultAsync();

                        _context.Subscriptions.RemoveRange(SubObj);
                        _context.SaveChanges();
                        return Page();
                    }

                    catch (Exception)
                    {
                        return RedirectToPage("SomethingwentError");
                    }
                }


            }
            catch (Exception)
            {

                return RedirectToPage("SomethingWentWrong");

            }

        }
    }
}
