using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Gameapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace Gameapp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]

    public class AddShopModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        private GamesContext _context;


        public AddShopModel(GamesContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IToastNotification toastNotification,
            ApplicationDbContext db,
            IWebHostEnvironment host)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _host = host;
            _toastNotification = toastNotification;
        }


        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.ViewModels.ShopVm shopVM { get; set; }

        
        public async Task<IActionResult> OnPostAsync(Models.ViewModels.ShopVm shopVM)
        
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var emailExist = _db.ApplicationUsers.FirstOrDefault(a => a.Email == shopVM.Email);

                if (emailExist != null)
                {
                    ModelState.AddModelError("DuplicateEmail", "Shop Email Already Exists");
                }


                if (ModelState.IsValid)
                {
                    Models.Shop model = new Models.Shop()
                    {
                        Address = shopVM.Address,
                        Email = shopVM.Email,
                        ShopNo = shopVM.ShopNo,
                        Password = shopVM.Password,
                        Mobile = shopVM.Mobile,
                        Deliverycost = shopVM.Deliverycost,
                        UserName = shopVM.UserName,
                        IsActive = shopVM.IsActive,
                        ShopTlar = shopVM.ShopTlar,
                        ShopTlen = shopVM.ShopTlen,
                        Tele = shopVM.Tele,
                        RegisterDate = DateTime.Now,
                        OrderIndex = shopVM.OrderIndex,
                        CountryId = shopVM.CountryId,
                    };

                    var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
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

                            if (HttpContext.Request.Form.Files[i].Name == "myFile")
                            {
                                model.Banner = uniqeFileName;


                            }
                            else
                            {
                                model.Pic = uniqeFileName;
                            }


                        }
                        await _userManager.AddToRoleAsync(user, "Shop");
                        if (uniqeFileName.Length <= 1)
                        {
                            DefaultAvatar def = new DefaultAvatar(_host);

                            uniqeFileName = def.CreateProfilePicture(shopVM.ShopTlen);
                        }
                       
                        _context.Shop.Add(model);
                        await _context.SaveChangesAsync();
                        var shop = _db.ApplicationUsers.FirstOrDefault(c => c.Email == model.Email);
                        shop.EntityId = model.ShopId;
                        shop.Pic = model.Pic;
                        _db.SaveChanges();
                        var subscription = new Gameapp.Models.Subscriptions()
                        {
                            PlanId = shopVM.PlanId.Value,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddMonths(_context.Plans.FindAsync(shopVM.PlanId.Value).Result.Period),
                            ShopId = user.EntityId,
                            Active = true

                        };
                        _context.Subscriptions.Add(subscription);
                        _context.SaveChanges();
                        _toastNotification.AddSuccessToastMessage("Shop Added Successfully");
                        return Redirect("/Admin/Shops");
                    }

                    ModelState.AddModelError("CreateUserError", result.Errors.First().Description);
                }

               

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
               
            }
            return Page();


        }
    }
}
