using Gameapp.Data;
using Gameapp.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Gameapp.Pages
{
    public class PrivacyModel : PageModel
    {
       

      

        public PageContent content { get; set; }
        private GamesContext _context;
        public IRequestCultureFeature locale;
        public string BrowserCulture;
        public string ContentAr { get; set; }

        public string ContentEn { get; set; }
        public PrivacyModel(GamesContext context)
        {
            _context = context;

        }
        public void OnGet()
        {
            locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            BrowserCulture = locale.RequestCulture.UICulture.ToString();
            var pageContent = _context.PageContent.FirstOrDefault(p => p.Id == 3);
            if (pageContent != null)
            {
                ContentAr = pageContent.Content;
                ContentEn = pageContent.Content;

            }
        }
    }
}
