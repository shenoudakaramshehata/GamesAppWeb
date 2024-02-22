using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Gameapp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace Gameapp.Areas.Admin.Pages.Coupon
{
    public class AddModel : PageModel
    {
        private GamesContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public AddModel(GamesContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }

        public void OnGet()
        {


        }
        public IActionResult OnPost(Models.Coupon model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (model.CouponTypeId == 0)
            {
                ModelState.AddModelError("CouponType", "Coupon Type Is Required..");
                return Page();
            }
            if (model.IssueDate > model.ExpirationDate)
            {
                ModelState.AddModelError("DateError", "Expiration Date must be greater than Issue Date...");
                return Page();

            }
            try
            {

                _context.Coupon.Add(model);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Coupon Added successfully");

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }

            return Redirect("./Index");
        }
    }
}