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

namespace Gameapp.Areas.Admin.Pages.ChampionsTypes
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
        public ChampionType championType { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                championType = await _context.ChampionType.FirstOrDefaultAsync(m => m.Id == id);
                if (championType == null)
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

                championType = await _context.ChampionType.FindAsync(id);
                if (championType != null)
                {
                    
                    _context.ChampionType.Remove(championType);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Champion Type Deleted successfully");
                  
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
