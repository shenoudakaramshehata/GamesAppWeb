using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Gameapp.Data;
using Gameapp.Models;
using Microsoft.EntityFrameworkCore;

namespace Gameapp.Areas.Admin.Pages.Banners
{
    public class IndexModel : PageModel
    {

        private GamesContext _context;
        public IndexModel(GamesContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public List<Banner> BannerLst { get; set; }

        public void OnGet()
        {
            BannerLst = _context.Banner.Include(s => s.Country).ToList();
            foreach (var item in BannerLst)
            {
                if (item.EntityId==null)
                  continue;
                
                if (item.SliderTypeId==1)
                {
                    var id = Convert.ToInt32(item.EntityId);
                  
                    item.EntityId = _context.Items.FirstOrDefault(c => c.ItemId == id)?.ItemName;
                }
                if (item.SliderTypeId == 2)
                {
                    var id = Convert.ToInt32(item.EntityId);
                    item.EntityId = _context.Shop.FirstOrDefault(c => c.ShopId == id)?.ShopTlen;
                }
                if (item.SliderTypeId == 3)
                {
                    var id = Convert.ToInt32(item.EntityId);
                    item.EntityId = _context.Champions.FirstOrDefault(c => c.ChampionId == id)?.ChampionTlEn;
                }
            

            }
        }
    }
}
