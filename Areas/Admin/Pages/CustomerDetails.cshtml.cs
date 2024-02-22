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

namespace Gameapp.Areas.Admin.Pages
{
    public class CustomerDetailsModel : PageModel
    {
        private GamesContext _context;

        private readonly IToastNotification _toastNotification;
        public CustomerDetailsModel(GamesContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        [BindProperty]
        public Models.Customer customer { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {
                customer = await _context.Customer.Include(c => c.ShippingAddress).FirstOrDefaultAsync(m => m.CustomerId == id);

                if (customer == null)
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
