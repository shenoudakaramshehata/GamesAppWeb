using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gameapp.Data;
using Gameapp.Models;

namespace Gameapp.Areas.Shop.Pages.Subscription
{
    public class DetailsModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;

        public DetailsModel(Gameapp.Data.GamesContext context)
        {
            _context = context;
        }

        public Subscriptions Subscriptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subscriptions = await _context.Subscriptions
                .Include(s => s.Plan)
                .Include(s => s.Shop).FirstOrDefaultAsync(m => m.Id == id);

            if (Subscriptions == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
