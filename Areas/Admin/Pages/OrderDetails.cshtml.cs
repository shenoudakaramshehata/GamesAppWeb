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

    public class OrderDetailsModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;

        public OrderDetailsModel(Gameapp.Data.GamesContext context)
        {
            _context = context;
        }

        public Models.ViewModels.OrderDetailsViewModel OrderDetailsViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

             OrderDetailsViewModel = new Models.ViewModels.OrderDetailsViewModel();

            OrderDetailsViewModel.Order = await _context.Order
                .Include(o => o.Customer)
                .Include(o => o.PaymentMehodPaymentMethod)
                .Include(o => o.ShippingAddress)
                .Include(o => o.Coupon)
                .Include(o => o.CouponType)
                .Include(o => o.shop).FirstOrDefaultAsync(m => m.OrderId == id);

            if (OrderDetailsViewModel.Order == null)
            {
                return NotFound();
            }


            OrderDetailsViewModel.OrderItem = _context.OrderItems.Include(s => s.Item).Where(s => s.OrderId == id).ToList();

            return Page();
        }
    }
}
