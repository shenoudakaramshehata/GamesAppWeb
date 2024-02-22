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
using Gameapp.Models.Notification;
using NToastNotify;

namespace Gameapp.Areas.Admin.Pages.PublicNotifications
{
    public class SendModel : PageModel
    {
        private GamesContext _context;
        private readonly INotificationService _notificationService;
        private readonly IToastNotification _toastNotification;

        public SendModel(GamesContext context, INotificationService notificationService, IToastNotification toastNotification)
        {
            _context = context;
            _notificationService = notificationService;
            _toastNotification = toastNotification;


        }
        [BindProperty]
        public PublicNotification publicNotification { get; set; }

        public IActionResult OnGetAsync(int id)
        {
            try
            {
                publicNotification = _context.PublicNotifications.Include(c => c.SliderType).Include(c => c.Country).Where(c => c.PublicNotificationId == id).FirstOrDefault();

                if (publicNotification == null)
                {
                    return Redirect("../Error");
                }

                if (publicNotification.SliderTypeId == 1)
                {
                    publicNotification.EntityNameAr = _context.Items.FirstOrDefault(c => c.ItemId == publicNotification.EntityId)?.ItemNameAr;
                    publicNotification.EntityNameEn = _context.Items.FirstOrDefault(c => c.ItemId == publicNotification.EntityId)?.ItemName;
                }
                if (publicNotification.SliderTypeId == 2)
                {
                    publicNotification.EntityNameAr = _context.Shop.FirstOrDefault(c => c.ShopId == publicNotification.EntityId)?.ShopTlar;
                    publicNotification.EntityNameEn = _context.Shop.FirstOrDefault(c => c.ShopId == publicNotification.EntityId)?.ShopTlen;
                }
                if (publicNotification.SliderTypeId == 3)
                {
                    publicNotification.EntityNameAr = _context.Champions.FirstOrDefault(c => c.ChampionId == publicNotification.EntityId)?.ChampionTlAR;
                    publicNotification.EntityNameEn = _context.Champions.FirstOrDefault(c => c.ChampionId == publicNotification.EntityId)?.ChampionTlEn;
                }

        }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {



            try
            {
                var publicNotificationModel = _context.PublicNotifications.Find(id);
                if (publicNotificationModel != null)
                {

                    var PublicDeviceList = _context.PublicDevices.Where(c => c.CountryId == publicNotificationModel.CountryId).ToList();
                    foreach (var item in PublicDeviceList)
                    {

                        var notificationModel = new NotificationModel();
                        notificationModel.DeviceId = item.DeviceId;
                        notificationModel.IsAndroiodDevice = item.IsAndroiodDevice;
                        notificationModel.Title = publicNotificationModel.Title;
                        notificationModel.Body = publicNotificationModel.Body;
                        notificationModel.IsAndroiodDevice = item.IsAndroiodDevice;
                        notificationModel.EntityId = publicNotificationModel.EntityId;
                        notificationModel.SliderTypeId= publicNotificationModel.SliderTypeId;
                        var result = await _notificationService.SendNotification(notificationModel);
                        if (result.IsSuccess)
                        {
                            var publicNotificationDeviceExiest = _context.PublicNotificationDevices.Any(c => c.PublicNotificationId == publicNotificationModel.PublicNotificationId
                             && c.PublicDeviceId == item.PublicDeviceId);
                            if (!publicNotificationDeviceExiest)
                            {
                                var publicNotificationDevice = new PublicNotificationDevice()
                                {
                                    PublicNotificationId = publicNotificationModel.PublicNotificationId,
                                    PublicDeviceId = item.PublicDeviceId,
                                    IsRead = false
                                };
                                _context.Add(publicNotificationDevice);
                                _context.SaveChanges();
                            }
                           

                        }
                    }

                }
                _toastNotification.AddSuccessToastMessage("Notification Sent successfully");
            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Sothing want Be Wrong");

                return Page();

            }

            return RedirectToPage("./Index");
        }
    }
}
