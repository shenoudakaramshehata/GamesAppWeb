using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gameapp.Data;
using Gameapp.Models;
using Microsoft.AspNetCore.Authorization;

namespace Gameapp.Areas.Admin.Pages.Items
{

    [Authorize(Roles = "Admin")]

    public class ItemDetailsModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;

        public ItemDetailsModel(Gameapp.Data.GamesContext context)
        {
            _context = context;
        }

        public Gameapp.Models.Items Items { get; set; }
        public static int shopId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Items = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.Customer)
                .Include(i => i.Shop)
                .Include(i => i.SubCategory).FirstOrDefaultAsync(m => m.ItemId == id);

            if (Items == null)
            {
                return NotFound();
            }
          
            return Page();
        }
    }
}
