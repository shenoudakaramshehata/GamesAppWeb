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

namespace Gameapp.Areas.Admin.Pages.Plans
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
        public Plan plan { get; set; }



        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {

                plan = await _context.Plans.FirstOrDefaultAsync(m => m.Id == id);
                if (plan == null)
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {

                var model = _context.Plans.Where(c => c.Id == id).FirstOrDefault();
                if (model == null)
                {
                    return Redirect("../NotFound");
                }
               
                model.ArabicTitle = plan.ArabicTitle;
                model.EnglishTitle = plan.EnglishTitle;
                model.NoOfItems = plan.NoOfItems;
                model.Period = plan.Period;
                model.Price = plan.Price;
                model.Reports = plan.Reports;
                model.AdzBanners = plan.AdzBanners;
                model.CountryId = plan.CountryId;
                model.Dashboard = plan.Dashboard;
                model.VipCollection = plan.VipCollection;

                _context.Attach(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Plan Edited successfully");


            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return RedirectToPage("./List");
        }
    }
}
