using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Controllers
{
    //[Route("Api/Localization")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    //public class LocalizationController : Controller
    //{
    //    [Route("SetLang")]
    //    public IActionResult SetLang(string culture, string url)
    //    {
    //        Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
    //             CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
    //             new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) }
    //             );
    //        return Redirect("~" + url);



    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LocalizationController : Controller
    {
      

 
        [HttpGet]
        public IActionResult SetLang(string culture, string url)

        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) }
                );
            return Redirect("~" + url);
        }
    
    }

    }

