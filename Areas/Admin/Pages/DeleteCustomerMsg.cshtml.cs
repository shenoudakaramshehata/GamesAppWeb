using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gameapp.Data;
using Gameapp.Models;
using NToastNotify;

namespace Gameapp.Areas.Admin
{
    public class DeleteCustomerMsgModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;
        private readonly IToastNotification _toastNotification;
        public DeleteCustomerMsgModel(Gameapp.Data.GamesContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public ContactUsMessages ContactUsMessages { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContactUsMessages = await _context.ContactUsMessages.FirstOrDefaultAsync(m => m.Id == id);

            if (ContactUsMessages == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                ContactUsMessages = await _context.ContactUsMessages.FindAsync(id);

                if (ContactUsMessages != null)
                {
                    _context.ContactUsMessages.Remove(ContactUsMessages);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Customer Message Deleted Successfully");
                }

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
           
            return Redirect("/Admin/ContactUsMessages");
        }
    }
}

