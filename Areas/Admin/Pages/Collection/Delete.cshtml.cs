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

namespace Gameapp.Areas.Admin.Pages.Collection
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
        public Gameapp.Models.Collection collection { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                collection = await _context.Collections.FirstOrDefaultAsync(m => m.CollectionId == id);
                if (collection == null)
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

                collection = await _context.Collections.FindAsync(id);
                
                    _context.Collections.Remove(collection);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Collection Deleted successfully");
                    
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
