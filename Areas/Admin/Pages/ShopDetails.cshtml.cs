using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gameapp.Data;
using Gameapp.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;

namespace Gameapp.Areas.Admin.Pages
{
    public class ShopDetailsModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;

        public ShopDetailsModel(Gameapp.Data.GamesContext context)
        {
            _context = context;
        }

        public Gameapp.Models.Shop Shop { get; set; }

        public JsonResult Subscriptions(DataSourceLoadOptions options)
        {
            
            return new JsonResult(DataSourceLoader.Load(_context.Subscriptions, options));
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Shop = await _context.Shop
                .Include(s => s.Subscriptions)
                .Include(s => s.Country)
                .Include(s => s.SubCategory).FirstOrDefaultAsync(m => m.ShopId == id);

            if (Shop == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
