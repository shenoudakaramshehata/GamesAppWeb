using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Gameapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;


namespace Gameapp.Areas.Admin.Pages.Currencey
{
    public class DetailsModel : PageModel
    {
        private GamesContext _context;


        private readonly IToastNotification _toastNotification;
        public DetailsModel(GamesContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        [BindProperty]
        public Curreny curreny { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {
                curreny = await _context.Curreny.FirstOrDefaultAsync(m => m.CurrenyId == id);

                if (curreny == null)
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

    }
}
