using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Gameapp.Data;
using Gameapp.Models;
using Microsoft.AspNetCore.Localization;
using NToastNotify;

namespace Gameapp.Areas.Admin.Pages.PublicNotifications
{
    public class IndexModel : PageModel
    {
        private GamesContext _context;
        private readonly IToastNotification _toastNotification;


        public IndexModel(GamesContext context,  IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        [BindProperty(SupportsGet = true)]
        public List<PublicNotification> PublicNotificationLst { get; set; }

        public void OnGet()
        {
            try
            {

                PublicNotificationLst = _context.PublicNotifications.ToList();

                foreach (var item in PublicNotificationLst)
                {



                    if (item.SliderTypeId == 1)
                    {
                        item.EntityNameAr = _context.Items.FirstOrDefault(c => c.ItemId == item.EntityId)?.ItemNameAr;
                        item.EntityNameEn = _context.Items.FirstOrDefault(c => c.ItemId == item.EntityId)?.ItemName;
                    }
                    if (item.SliderTypeId == 2)
                    {
                        item.EntityNameAr = _context.Shop.FirstOrDefault(c => c.ShopId == item.EntityId)?.ShopTlar;
                        item.EntityNameEn = _context.Shop.FirstOrDefault(c => c.ShopId == item.EntityId)?.ShopTlen;
                    }
                    if (item.SliderTypeId ==3)
                    {
                        item.EntityNameAr = _context.Champions.FirstOrDefault(c => c.ChampionId == item.EntityId)?.ChampionTlAR;
                        item.EntityNameEn = _context.Champions.FirstOrDefault(c => c.ChampionId == item.EntityId)?.ChampionTlEn;
                    }


                }
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }
        }
        }
}
