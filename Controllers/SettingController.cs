using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Gameapp.Data;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.Extensions.Logging;

namespace Gameapp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SettingController : Controller
    {
       
        private readonly ILogger<LogoutModel> _logger;

        public SettingController( ILogger<LogoutModel> logger)
        {
            
            _logger = logger;
        }
        [HttpGet]
        public IActionResult ChangeLanguage(string culture, string url)

        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) }
                );
            return Redirect("~" + url);
        }
       

    }
}
