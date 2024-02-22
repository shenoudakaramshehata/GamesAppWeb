using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Gameapp.Data;
using Gameapp.Models;
using Gameapp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gameapp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]

    public class UserManagementModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;
        private readonly ApplicationDbContext _db;
        //private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementModel(Gameapp.Data.GamesContext context,
            ApplicationDbContext db,
            //RoleManager<ApplicationUser> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _db = db;
            //_roleManager = roleManager;
            _userManager = userManager;
        }

        public IList<UsersManagement> UserManagment { get; set; }



        public async Task<JsonResult> OnGetUsers(DataSourceLoadOptions options)
        {

            var m = _db.ApplicationUsers;

            List<UsersManagement> lst = new List<UsersManagement>();


            var users = _db.ApplicationUsers.ToList();

            foreach (var item in users)
            {
                UsersManagement rec = new UsersManagement();
                rec.Id = item.Id;
                rec.Email = item.Email;
                rec.UserName = item.UserName;

                //Role
                var userRoles = _userManager.GetRolesAsync(item);
                if (userRoles != null)
                {
                    var OnlyRole = userRoles.Result.FirstOrDefault();
                    if (OnlyRole != null)
                    {
                        switch (OnlyRole)
                        {
                            case "Admin":
                                rec.Role = OnlyRole;
                                rec.Title = item.UserName;
                                break;
                            case "Shop":
                                rec.Role = OnlyRole;
                                var driverRecord = _context.Shop.FirstOrDefault(u => u.ShopId == item.EntityId);
                                if (driverRecord != null)
                                {
                                    rec.Title = driverRecord.ShopTlar;
                                }
                                break;
                            case "Customer":
                                rec.Role = OnlyRole;
                                var clientRecord = _context.Customer.FirstOrDefault(u => u.CustomerId == item.EntityId);
                                if (clientRecord != null)
                                {
                                    rec.Title = clientRecord.CustomernameEn;
                                }
                                break;
                            default:
                                rec.Role = "";
                                rec.Title = "";
                                break;
                        }
                    }





                }

                lst.Add(rec);
            }

            //UserManagment =  _db.ApplicationUsers.Select(u =>  new ReportModels.UsersManagement { 

            //    Id = u.Id,
            //    Email = u.Email,
            //    UserName = u.UserName,
            //    Role = _userManager.GetRolesAsync(_db.ApplicationUsers.FirstOrDefault(a => a.UserName == u.UserName)).Result.ElementAt(0),
            //    Title = FindTitle(_userManager.GetRolesAsync(_db.ApplicationUsers.FirstOrDefault(a => a.UserName == u.UserName)).Result.ElementAt(0), u.EntityId, u.Id)

            //}).ToList();

            return new JsonResult(DataSourceLoader.Load(lst, options));
        }

        public string FindTitle(string roleName, int entityId, string id)
        {

            switch (roleName)
            {
                case "Admin":
                    //return _db.ApplicationUsers.FirstOrDefault(u => u.Id == id).UserName;
                    return "Admin";
                case "Shop":
                    return _context.Shop.FirstOrDefault(u => u.ShopId == entityId).ShopTlen;
                case "Customer":
                    return _context.Customer.FirstOrDefault(u => u.CustomerId == entityId).CustomernameEn;
                default:
                    return "null";
            }
        }
    }
}
