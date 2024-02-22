using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gameapp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class AddBannerModel : PageModel
    {
        private readonly IWebHostEnvironment _host;
        private GamesContext _context;


        public AddBannerModel(GamesContext context,
            ApplicationDbContext db,
            IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        [BindProperty]
        public Models.Banner Banner { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }


    }
}
