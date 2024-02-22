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
using Microsoft.AspNetCore.Hosting;
using NToastNotify;
using System.IO;

namespace Gameapp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]

    public class DeleteShopModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public DeleteShopModel(Gameapp.Data.GamesContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }

        [BindProperty]
        public Gameapp.Models.Shop Shop { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Shop = await _context.Shop
                .Include(s => s.SubCategory)
                .Include(s => s.Items)
                .Include(s => s.Country)
                .FirstOrDefaultAsync(m => m.ShopId == id);

            if (Shop == null)
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

            Shop = await _context.Shop.Include(e=>e.Country).Include(s => s.SubCategory)
                .Include(s => s.Items).FirstOrDefaultAsync(e=>e.ShopId==id);

            if (Shop != null)
            {
                var itemsList = _context.Items.Where(e => e.ShopId == id).ToList();
                if (itemsList != null) 
                {
                    foreach (var item in itemsList)
                    {
                        var imageList = _context.ProductImages.Where(e => e.ItemId == item.ItemId).ToList();
                        if (imageList != null)
                        {
                            _context.ProductImages.RemoveRange(imageList);
                        }
                        var shoppingList = _context.ShoppingCart.Where(e => e.ItemId == item.ItemId).ToList();
                        if (shoppingList != null)
                        {
                            _context.ShoppingCart.RemoveRange(shoppingList);
                        }
                        var wilshList = _context.WishList.Where(e => e.ItemId == item.ItemId).ToList();
                        if (wilshList != null)
                        {
                            _context.WishList.RemoveRange(wilshList);
                        }
                    }
                    // imageList = _context.ProductImages.Where(e => e.ItemId == id).ToList();
                    _context.Items.RemoveRange(itemsList);
                }
                _context.Shop.Remove(Shop);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Shop Deleted successfully");
                var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/" + Shop.Pic);
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
            }
            else
            {
                return Redirect("../Error");
            }
          
        

            return Redirect("/admin/shops");
        }
    }
}
