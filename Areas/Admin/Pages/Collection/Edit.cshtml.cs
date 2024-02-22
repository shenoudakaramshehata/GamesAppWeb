using System;
using System.Collections.Generic;
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

                var model = _context.Collections.Where(c => c.CollectionId == id).FirstOrDefault();
                if (model == null)
                {
                    return Redirect("../NotFound");
                }
                 model.IsActive = collection.IsActive;
                model.CollectionTitleAr = collection.CollectionTitleAr;
                model.CollectionTitleEn = collection.CollectionTitleEn;
                model.Source = collection.Source;
                _context.Attach(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Collection Edited successfully");


            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return RedirectToPage("./Index");
        }
    }
}
