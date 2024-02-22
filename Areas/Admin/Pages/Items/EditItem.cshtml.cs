using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Gameapp.Models;
using Gameapp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gameapp.Areas.Admin.Pages.Items
{
    [Authorize(Roles = "Admin")]

    public class EditItemModel : PageModel
    {
        private readonly GamesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _host;

        public EditItemModel(GamesContext context, UserManager<ApplicationUser> userManage, IWebHostEnvironment host)
        {
            _context = context;
            _userManager = userManage;
            _host = host;
        }

        [BindProperty]
        public Gameapp.Models.Items ItemImagesAndItemVm { get; set; }

        public void OnGet(int id)
        {
                ItemImagesAndItemVm = _context.Items.FirstOrDefault(i => i.ItemId == id);

           
        }



        public async Task<IActionResult> OnPostAsync(Gameapp.Models.Items ItemImagesAndItemVm)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //var shopId = _userManager.FindByNameAsync(User.Identity.Name).Result.EntityId;
            //ItemImagesAndItemVm.ShopId = shopId;

            //_context.Items.Add(new Items());
            //await _context.SaveChangesAsync();

            if(_context.Items.FirstOrDefault(i => i.ItemId == ItemImagesAndItemVm.ItemId) != null)
            {
               var item = _context.Items.FirstOrDefault(i => i.ItemId == ItemImagesAndItemVm.ItemId);
                item.ItemName = ItemImagesAndItemVm.ItemName;
                item.ItemPrice = ItemImagesAndItemVm.ItemPrice;
                item.ItemDescription = ItemImagesAndItemVm.ItemDescription;
                item.CategoryId = ItemImagesAndItemVm.CategoryId;
                item.IsActive = ItemImagesAndItemVm.IsActive;
                item.OrderIndex = ItemImagesAndItemVm.OrderIndex;
                item.SubCategoryId = ItemImagesAndItemVm.SubCategoryId;
                item.OutOfStock = ItemImagesAndItemVm.OutOfStock;
                item.ShopId = ItemImagesAndItemVm.ShopId;

                //var shopId = _userManager.FindByNameAsync(User.Identity.Name).Result.EntityId;
                var itemCountry = _context.Shop.FirstOrDefault(s => s.ShopId == ItemImagesAndItemVm.ShopId).CountryId;


                item.CountryId = itemCountry;
                _context.SaveChanges();

                if (HttpContext.Request.Form.Files.Count() > 0)
                {
                    var uniqeFileName = "";

                    for (int i = 0; i < HttpContext.Request.Form.Files.Count(); i++)
                    {

                        string uploadFolder = Path.Combine(_host.WebRootPath, "images");

                        uniqeFileName = Guid.NewGuid() + "_" + Response.HttpContext.Request.Form.Files[i].FileName;

                        string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);

                        using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                        {
                            Response.HttpContext.Request.Form.Files[i].CopyTo(fileStream);
                        }

                        if (HttpContext.Request.Form.Files[i].Name == "MainImage")
                        {
                            item.ItemImage = uniqeFileName;
                        }
                        else
                        {
                            ProductImages proImg = new ProductImages()
                            {
                                ImageName = uniqeFileName,
                                ItemId = ItemImagesAndItemVm.ItemId
                            };

                            _context.ProductImages.Add(proImg);

                        }

                    }

                    _context.SaveChanges();
                }

            }

            
            return Redirect("/Admin/ShopDetails?id=" + ItemImagesAndItemVm.ShopId);
        }
    }
}
