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
using NToastNotify;

namespace Gameapp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    
    public class EditShopModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        private GamesContext _context;
        private bool isChanged = false;
        private IToastNotification  _toastNotification;
        private IHttpContextAccessor _httpContextAccessor;
        public static int shopId { get; set; }

        public EditShopModel(GamesContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            IWebHostEnvironment host, IHttpContextAccessor httpContextAccessor, IToastNotification toastNotification)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _host = host;
            _httpContextAccessor = httpContextAccessor;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public Gameapp.Models.Shop Shop { get; set; }
        public IActionResult OnGet(int id)
        {
            try
            {
                Shop = _context.Shop.Find(id);
                Shop.Email = _db.ApplicationUsers.FirstOrDefault(s => s.EntityId == id).Email;
                shopId = id;
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something Went Error");
            }
            return Page();

        }
        public async Task<IActionResult> OnPostAsync(Gameapp.Models.Shop Shop)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var oldShop = _db.ApplicationUsers.FirstOrDefault(a => a.EntityId == Shop.ShopId);

                if (ModelState.IsValid)
                {

                    var user = _db.ApplicationUsers.FirstOrDefault(s => s.EntityId == Shop.ShopId);

                    if (user != null)
                    {
                        user.Email = Shop.Email;

                        var uniqeFileName = "";

                        if (Response.HttpContext.Request.Form.Files.Count() > 0)
                        {
                            for (int i = 0; i < HttpContext.Request.Form.Files.Count(); i++)
                            {
                                string uploadFolder = Path.Combine(_host.WebRootPath, "images");

                                uniqeFileName = Guid.NewGuid() + "_" + Response.HttpContext.Request.Form.Files[0].FileName;

                                string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);

                                using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                                {
                                    Response.HttpContext.Request.Form.Files[i].CopyTo(fileStream);
                                }
                                if (HttpContext.Request.Form.Files[i].Name == "Pic")
                                {
                                    Shop.Pic = uniqeFileName;
                                    user.Pic = uniqeFileName;
                                    isChanged = true;

                                }
                                else if (HttpContext.Request.Form.Files[i].Name == "Banner")
                                {
                                    Shop.Banner = uniqeFileName;
                                }
                            }
                        }
                        else if(Response.HttpContext.Request.Form.Files.Count() == 0 ||isChanged==false)
                        {

                            var oldPictureIsDefault = _context.Shop.AsNoTracking().FirstOrDefault(s => s.ShopId == oldShop.EntityId).Pic.Contains("defaultPicture535");
                            if (oldPictureIsDefault)
                            {

                                DefaultAvatar def = new DefaultAvatar(_host);

                                Shop.Pic = def.CreateProfilePicture(Shop.ShopTlen);

                            }
                            else
                            {

                                Shop.Pic = oldShop.Pic;
                            }

                        }
                    
                        
                    }

                    _db.SaveChanges();
                    _context.Attach(Shop).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Shop Edited Successfully");
                  
                }
                return Redirect("Shops");

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something Went Error");
            }
            
            return Page();
        }


    }
}

