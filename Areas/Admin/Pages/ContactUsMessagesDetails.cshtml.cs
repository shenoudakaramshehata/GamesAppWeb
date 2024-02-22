using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gameapp.Data;
using Gameapp.Models;

namespace Gameapp.Areas.Admin.Pages
{
    public class ContactUsMessagesDetailsModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;

        public ContactUsMessagesDetailsModel(Gameapp.Data.GamesContext context)
        {
            _context = context;
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
    }
}
