using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gameapp.Areas.Shop.Pages.Orders
{
    [Authorize(Roles = "Admin,Shop")]

    public class OrderDetailsModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderDetailsModel(Gameapp.Data.GamesContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            // to make sure that order details belong to this shop and not another shop

            var entityId = _userManager.FindByNameAsync(User.Identity.Name).Result.EntityId;

            if (OrderDetailsViewModel.Order.ShopId != entityId || OrderDetailsViewModel.Order == null)
            {
                return NotFound();
            }


            OrderDetailsViewModel.OrderItem = _context.OrderItems.Include(s => s.Item).Where(s => s.OrderId == id).ToList();

            return Page();
        }
    }
}
