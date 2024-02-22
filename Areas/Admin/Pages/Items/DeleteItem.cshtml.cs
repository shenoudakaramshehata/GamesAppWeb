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

    public class DeleteItemModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;

        public DeleteItemModel(Gameapp.Data.GamesContext context)
        {
            _context = context;
        }
        public static int ShopId { get; set; }
        [BindProperty]
        public Gameapp.Models.Items Items { get; set; }

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
            ShopId = Items.ShopId.Value;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newItems = await _context.Items.FindAsync(id);

            if (newItems != null)
            {
                var images = _context.ProductImages.Where(e => e.ItemId == newItems.ItemId).ToList();
                if (images != null)
                {
                    _context.ProductImages.RemoveRange(images);
                }
                _context.Items.Remove(newItems);
                await _context.SaveChangesAsync();
            }

            return Redirect("/Admin/ShopDetails?id=" +ShopId);
        }
    }
}
