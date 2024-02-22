using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gameapp.Pages
{
    public class PlansModel : PageModel
    {
        private readonly GamesContext _context;

        public PlansModel(Gameapp.Data.GamesContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<Models.Plan> Plans { get; set; }
        public void OnGet(int id)
        {
            Plans = _context.Plans.Where(s => s.CountryId == id).ToList();
        }
    }
}
