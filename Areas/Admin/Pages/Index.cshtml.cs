using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gameapp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]

    public class IndexModel : PageModel
    {
        private readonly GamesContext _context;

        public IndexModel(Gameapp.Data.GamesContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]

        public string url { get; set; }
        public void OnGet()
        {
            url = $"{this.Request.Scheme}://{this.Request.Host}";

        }

    }
}
