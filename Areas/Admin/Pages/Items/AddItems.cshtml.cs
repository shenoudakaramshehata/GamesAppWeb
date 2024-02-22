using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Gameapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gameapp.Areas.Admin.Pages.Items
{
    [Authorize(Roles = "Admin")]

    public class ItemsModel : PageModel
    {
       
        private readonly GamesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _host;
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public Gameapp.Models.Items Item { get; set; }

        public ItemsModel(GamesContext context, UserManager<ApplicationUser> userManage, IWebHostEnvironment host)
        {
            _context = context;
            _userManager = userManage;
            _host = host;
        }
        public void OnGet(int id)
        {
            Id = id;
        }
        

        public async Task<IActionResult> OnPostAsync(Gameapp.Models.Items Item)
        {
           // var shopID = _userManager.FindByNameAsync(User.Identity.Name).Result.EntityId;
            var lastSubscription = _context.Subscriptions.Include(q => q.Plan).Where(s => s.ShopId ==Id).OrderBy(i => i.Id).LastOrDefault();
            var numberOfAvailableItems = lastSubscription.Plan.NoOfItems;

            var numberOfCurrentItems = _context.Items.Where(s => s.ShopId == Id).Count();
            

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (numberOfCurrentItems >= numberOfAvailableItems)
            {
                ModelState.AddModelError("Exceed", "you have exceeded the number of available items");
                return Page();
            }

            //var shopId = _userManager.FindByNameAsync(User.Identity.Name).Result.EntityId;
            Item.ShopId = Id;

            var itemCountry = _context.Shop.FirstOrDefault(s => s.ShopId == Id).CountryId;
            Item.CountryId = itemCountry;

            _context.Items.Add(Item);
            await _context.SaveChangesAsync();

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
                        Item.ItemImage = uniqeFileName;
                        _context.Attach(Item).State = EntityState.Modified;

                    }else
                    {
                    ProductImages proImg = new ProductImages()
                    {
                        ImageName = uniqeFileName,
                        ItemId = Item.ItemId
                    };

                        _context.ProductImages.Add(proImg);

                    }

                }

                _context.SaveChanges();
            }


            var collectionsCheckBox = Request.Form["Collection"];

            foreach (var item in collectionsCheckBox)
            {
                var collectionId = int.Parse(item);

                CollectionItem col = new CollectionItem()
                {

                    CollectionId = collectionId,
                    ItemId = Item.ItemId

                };

                _context.CollectionItems.Add(col);

            }

            _context.SaveChanges();

            return Redirect("/Admin/ShopDetails?id="+Id);
        }

    }
}
