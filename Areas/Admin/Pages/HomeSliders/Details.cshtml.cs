using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gameapp.Data;
using Gameapp.Models;
using NToastNotify;

namespace Gameapp.Areas.Admin.Pages.HomeSliders
{
    public class DetailsModel : PageModel
    {
        private GamesContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public DetailsModel(GamesContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public Slider slider { get; set; }
        [BindProperty]
        public int EntityId { get; set; }
        [BindProperty]
        public string EntityNameEn { get; set; }
        [BindProperty]
        public string EntityNameAr { get; set; }
        //public async Task<IActionResult> OnGetAsync(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    banner = _context.Banner.Where(c => c.BannerId == id).FirstOrDefault();
        //    if (banner == null)
        //    {
        //        return NotFound();
        //    }
        //    if (banner.SliderTypeId > 3)
        //    {
        //        EntityId = 0;

        //    }
        //    else
        //    {
        //        EntityId = int.Parse(banner.EntityId);
        //        banner.EntityId = null;

        //    }


        //    return Page();
        //}

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                slider = _context.Slider.Include(c => c.SliderType).Where(c => c.SliderId == id).FirstOrDefault();
                if (slider == null)
                {
                    return Redirect("../Error");
                }

                if (slider.SliderTypeId == 1)
                {
                    var EntityId = Convert.ToInt32(slider.EntityId);

                    EntityNameEn = _context.Items.FirstOrDefault(c => c.ItemId == EntityId)?.ItemName;
                    EntityNameAr = _context.Items.FirstOrDefault(c => c.ItemId == EntityId)?.ItemNameAr;
                }
                if (slider.SliderTypeId == 2)
                {
                    var EntityId = Convert.ToInt32(slider.EntityId);

                    EntityNameEn = _context.Shop.FirstOrDefault(c => c.ShopId == EntityId)?.ShopTlen;
                    EntityNameAr = _context.Shop.FirstOrDefault(c => c.ShopId == EntityId)?.ShopTlar;
                }
                if (slider.SliderTypeId == 3)
                {
                    var EntityId = Convert.ToInt32(slider.EntityId);

                    EntityNameEn = _context.Champions.FirstOrDefault(c => c.ChampionId == EntityId)?.ChampionTlEn;
                    EntityNameAr = _context.Champions.FirstOrDefault(c => c.ChampionId == EntityId)?.ChampionTlAR;
                }
                if (slider.SliderTypeId == 4)
                {
                    slider.EntityId = null;
                }

                if (slider.SliderTypeId == 5)
                {

                    EntityNameAr = slider.EntityId;
                    EntityNameEn = slider.EntityId;
                }


            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }


            return Page();
        }
        //public async Task<IActionResult> OnGetAsync(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    slider = _context.Slider.Where(c => c.SliderId == id).FirstOrDefault();
        //    if (slider == null)
        //    {
        //        return NotFound();
        //    }
        //    if (slider.SliderTypeId > 3)
        //    {
        //        EntityId = 0;

        //    }
        //    else
        //    {
        //        EntityId = int.Parse(slider.EntityId);
        //        slider.EntityId = null;

        //    }


        //    return Page();
        //}


    }
}