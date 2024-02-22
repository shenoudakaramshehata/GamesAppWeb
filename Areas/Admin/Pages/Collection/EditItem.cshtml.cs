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

namespace Gameapp.Areas.Admin.Pages.Collection
{
    [Authorize(Roles = "Admin")]
    public class EditItemModel : PageModel
    {
        private readonly GamesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _host;
        [BindProperty]
        public static int shopId { get; set; }
        public EditItemModel(GamesContext context, UserManager<ApplicationUser> userManage, IWebHostEnvironment host)
        {
            _context = context;
            _userManager = userManage;
            _host = host;
        }

        [BindProperty]
        public Gameapp.Models.Items ItemImagesAndItemVm { get; set; }
        [BindProperty]
        public List<int> CollectionList { get; set; }

        public void OnGet(int id)
        {
            
            ItemImagesAndItemVm = _context.Items.FirstOrDefault(i => i.ItemId == id);
            if (ItemImagesAndItemVm != null)
            {
                shopId = ItemImagesAndItemVm.ShopId.Value;
                
                 CollectionList = _context.CollectionItems.Where(e => e.ItemId == id).Select(e=>e.CollectionId).ToList();
            }
               
           

        }



        public async Task<IActionResult> OnPostAsync(Gameapp.Models.Items ItemImagesAndItemVm)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            

            if (_context.Items.FirstOrDefault(i => i.ItemId == ItemImagesAndItemVm.ItemId) != null)
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
            var collectionsCheckBox = Request.Form["Collection"];
            var collectionItemList = _context.CollectionItems.Where(e => e.ItemId == ItemImagesAndItemVm.ItemId).ToList();
            if (collectionItemList != null)
            {
                _context.CollectionItems.RemoveRange(collectionItemList);
            }

            foreach (var item in collectionsCheckBox)
            {
                var collectionId = int.Parse(item);

                CollectionItem col = new CollectionItem()
                {

                    CollectionId = collectionId,
                    ItemId = ItemImagesAndItemVm.ItemId

                };

                _context.CollectionItems.Add(col);

            }
            _context.SaveChanges();

            return Redirect("./index");
        }
    }
}
