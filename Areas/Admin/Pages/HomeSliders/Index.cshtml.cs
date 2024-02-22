using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Gameapp.Data;
using Gameapp.Models;
using Microsoft.EntityFrameworkCore;

namespace Gameapp.Areas.Admin.Pages.HomeSliders
{
    public class IndexModel : PageModel
    {

        private GamesContext _context;
        public IndexModel(GamesContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public bool ArLang { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<Slider> SliderLst { get; set; }

        public void OnGet()
        {
            SliderLst = _context.Slider.Include(s => s.Country).ToList();
            foreach (var item in SliderLst)
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
