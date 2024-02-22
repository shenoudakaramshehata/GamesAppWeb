using Gameapp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Models;

namespace Gameapp.Areas.Admin
{
    [Area("Admin")]
    [Route("[Area]/[Controller]/[Action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RemoteController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RemoteController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        [Route("userName")]
        public ActionResult CheckEmailExist([Bind(Prefix = "shopVM.Email")]  string userName)
       {
            var driverExist = _userManager.FindByEmailAsync(userName).Result;

            if (driverExist == null)
            {
                return Json(true);
            }

            return Json($"Email {userName} is already in use");

        }


        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        [Route("userName")]
        public ActionResult CheckUserNameExist([Bind(Prefix = "shopVM.UserName")] string userName)
        {
            var driverExist = _userManager.FindByNameAsync(userName).Result;


            if (driverExist == null)
            {
                return Json(true);
            }

            return Json($"username {userName} is already in use");

        }


        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        [Route("userName")]
        public ActionResult CheckBranchUserNameExist([Bind(Prefix = "UserName")] string userName)
        {
            var driverExist = _userManager.FindByNameAsync(userName).Result;


            if (driverExist == null)
            {
                return Json(true);
            }

            return Json($"username {userName} is already in use");

        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        [Route("password")]

        public ActionResult CheckClientPassword([Bind(Prefix = "Client.Password")] string password)
        {
           
            if (password != null && password.Length > 6)
            {
                return Json(true);
            }

            return Json($"password is very weak");

        }




    }
}
