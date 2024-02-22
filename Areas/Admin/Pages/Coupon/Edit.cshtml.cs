using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Gameapp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Gameapp.Areas.Admin.Pages.Coupon
{
    public class EditModel : PageModel
    {
        private GamesContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public EditModel(GamesContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
        [BindProperty]
        public Models.Coupon coupon { get; set; }



        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {

                coupon = await _context.Coupon.FirstOrDefaultAsync(m => m.Id == id);
                if (coupon == null)
                {
                    return Redirect("../NotFound");
                }

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }

            return Page();
        }


        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            try
            {
                var model = _context.Coupon.Where(c => c.Id == id).FirstOrDefault();
                if (model == null)
                {
                    return Redirect("../NorFound");

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

                model.Serial = coupon.Serial;
                model.Amount = coupon.Amount;
                model.IssueDate = coupon.IssueDate;
                model.ExpirationDate = coupon.ExpirationDate;
                model.CouponTypeId = coupon.CouponTypeId;
                _context.Attach(model).State = EntityState.Modified;
                 _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Coupon Edited successfully");


            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }

            return Redirect("./Index");
        }
    }
}
