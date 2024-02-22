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

namespace Gameapp.Areas.Admin.Pages.SubCategories
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
        public SubCategory subcategory { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {
                subcategory = await _context.SubCategory.Include(c => c.Category).FirstOrDefaultAsync(m => m.SubCategoryId == id);
                if (subcategory == null)
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
                subcategory = await _context.SubCategory.Include(c => c.Category).FirstOrDefaultAsync(m => m.SubCategoryId == id);

                _context.SubCategory.Remove(subcategory);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("SubCategories Deleted successfully");
                //var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/SubCategories/" + subcategory.SubCategoryPic);
                //if (System.IO.File.Exists(ImagePath))
                //{
                //    System.IO.File.Delete(ImagePath);
                //}
            
                //else
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
