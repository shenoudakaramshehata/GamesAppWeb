using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gameapp.Areas.Admin.Pages.Subscriptions
{
    public class AddModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private GamesContext _context;


        public AddModel(GamesContext context,
            ApplicationDbContext db)
        {
            _context = context;
            _db = db;
        }


        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public Models.Subscriptions Subscription { get; set; }
        public void OnGet(int id)
        {
            Id = id;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            Subscription.StartDate = DateTime.Now;
            Subscription.ShopId = Id;
            

            var plan = await _context.Plans.FindAsync(Subscription.PlanId);
            Subscription.EndDate = DateTime.Now.AddMonths(plan.Period);

            

            await _context.Subscriptions.AddAsync(Subscription);
            _context.SaveChanges();

            return LocalRedirect("/admin/subscriptions/list");
        }
    }
}
