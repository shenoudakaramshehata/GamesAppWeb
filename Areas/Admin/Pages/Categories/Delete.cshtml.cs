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

namespace Gameapp.Areas.Admin.Pages.Categories
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
        public Category category { get; set; }



        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {
                category = await _context.Category.FirstOrDefaultAsync(m => m.CategoryId == id);

                if (category == null)
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
                category = await _context.Category.FindAsync(id);
                if (category != null)
                {
                    var subCateList = _context.SubCategory.Where(e => e.CategoryId == id).ToList();
                    //if (_context.SubCategory.Any(c => c.CategoryId == id))
                    //{
                    //    _toastNotification.AddErrorToastMessage("You cannot delete this Category");

                    //    return Page();

                    //}
                    if (subCateList!=null)
                    {
                        _context.SubCategory.RemoveRange(subCateList);
                    }

                    _context.Category.Remove(category);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Category Deleted successfully");
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/categories/" + category.CategoryPic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                }
                else
                {
                    return Redirect("../Error");
                }
                var ImagePathIcon = Path.Combine(_hostEnvironment.WebRootPath, "Images/categories/" + category.CategoryIcon);
                if (System.IO.File.Exists(ImagePathIcon))
                {
                    System.IO.File.Delete(ImagePathIcon);
                }

                else { 
                return Redirect("../Error");
        }
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
