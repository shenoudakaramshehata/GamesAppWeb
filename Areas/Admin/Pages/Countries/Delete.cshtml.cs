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

namespace Gameapp.Areas.Admin.Pages.Countries
{
    public class DeleteModel : PageModel
    {
        private GamesContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public DeleteModel(GamesContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
        [BindProperty]
        public Country country { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                country = await _context.Country.FirstOrDefaultAsync(m => m.CountryId == id);
                if (country == null)
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
            try
            {

                country = await _context.Country.FindAsync(id);
                if (country != null)
                {
                    if (_context.Items.Any(c => c.CountryId == id))
                    {
                        _toastNotification.AddErrorToastMessage("You cannot delete this Country");
                        return Page();

                    }
                    if (_context.Banner.Any(c => c.CountryId == id))
                    {
                        _toastNotification.AddErrorToastMessage("You cannot delete this Country");
                        return Page();

                    }
                    if (_context.Shop.Any(c => c.CountryId == id))
                    {
                        _toastNotification.AddErrorToastMessage("You cannot delete this Country");
                        return Page();

                    }
                    _context.Country.Remove(country);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Country Deleted successfully");
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/countries/" + country.Pic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                }
                else
                    return Redirect("../NotFound");
            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");

                return Page();

            }

            return RedirectToPage("./Index");
        }

    }
}
