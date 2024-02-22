using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gameapp.Areas.Shop.Pages.Orders
{
    [Authorize(Roles = "Admin,Shop")]

    public class OrdersModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public bool ArLang { get; set; }
        public void OnGet()
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();
            if (BrowserCulture == "en-US")
                ArLang = false;
            else
                ArLang = true;

        }
    }
}
