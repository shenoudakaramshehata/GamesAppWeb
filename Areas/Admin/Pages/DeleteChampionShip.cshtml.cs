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
    public class DeleteChampionShipModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;

        public DeleteChampionShipModel(Gameapp.Data.GamesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Champion Champion { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Champion = await _context.Champions
                .Include(c => c.ChampionType)
                .Include(c => c.GameMode).FirstOrDefaultAsync(m => m.ChampionId == id);

            if (Champion == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Champion = await _context.Champions.FindAsync(id);

            if (Champion != null)
            {
                _context.Champions.Remove(Champion);
                await _context.SaveChangesAsync();
            }

            return Redirect("/admin/ChampionShip");
        }
    }
}
