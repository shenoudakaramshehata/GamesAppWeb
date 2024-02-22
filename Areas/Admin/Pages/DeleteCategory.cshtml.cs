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

namespace Gameapp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]

    public class DeleteCategoryModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;

        public DeleteCategoryModel(Gameapp.Data.GamesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Category
                .Include(c => c.SubCategory)
                .Include(c => c.Items)
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            if (Category == null)
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

            Category = await _context.Category.FindAsync(id);

            if (Category != null)
            {
                _context.Category.Remove(Category);
                await _context.SaveChangesAsync();
            }

            return Redirect("/Admin/Categories");
        }
    }
}
