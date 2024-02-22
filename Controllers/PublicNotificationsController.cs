using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Gameapp.Models;

namespace Gameapp.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PublicNotificationsController : Controller
    {
        private GamesContext _context;

        public PublicNotificationsController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var publicnotifications = _context.PublicNotifications.Select(i => new {
                i.PublicNotificationId,
                i.Title,
                i.Body,
                i.Date,
                i.SliderTypeId,
                i.EntityId,
                i.EntityNameAr,
                i.EntityNameEn,
                i.CountryId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PublicNotificationId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(publicnotifications, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new PublicNotification();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.PublicNotifications.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PublicNotificationId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.PublicNotifications.FirstOrDefaultAsync(item => item.PublicNotificationId == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.PublicNotifications.FirstOrDefaultAsync(item => item.PublicNotificationId == key);

            _context.PublicNotifications.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CountryLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Country
                         orderby i.CountryTlar
                         select new {
                             Value = i.CountryId,
                             Text = i.CountryTlar
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> SliderTypeLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.SliderType
                         orderby i.SliderTypeTlar
                         where i.SliderTypeId<4
                         select new {
                             Value = i.SliderTypeId,
                             Text = i.SliderTypeTlar
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }
        [HttpGet]
        public async Task<IActionResult> ItemLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Items
                         orderby i.ItemNameAr
                         select new {
                             Value = i.ItemId,
                             Text = i.ItemNameAr
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }
        [HttpGet]
        public async Task<IActionResult> ShopLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Shop
                         orderby i.ShopTlar
                         select new {
                             Value = i.ShopId,
                             Text = i.ShopTlar
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }
        [HttpGet]
        public async Task<IActionResult> ChampionsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Champions
                         orderby i.ChampionTlAR
                         select new {
                             Value = i.ChampionId,
                             Text = i.ChampionTlAR
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(PublicNotification model, IDictionary values) {
            string PUBLIC_NOTIFICATION_ID = nameof(PublicNotification.PublicNotificationId);
            string TITLE = nameof(PublicNotification.Title);
            string BODY = nameof(PublicNotification.Body);
            string DATE = nameof(PublicNotification.Date);
            string SLIDER_TYPE_ID = nameof(PublicNotification.SliderTypeId);
            string ENTITY_ID = nameof(PublicNotification.EntityId);
            string ENTITY_NAME_AR = nameof(PublicNotification.EntityNameAr);
            string ENTITY_NAME_EN = nameof(PublicNotification.EntityNameEn);
            string COUNTRY_ID = nameof(PublicNotification.CountryId);

            if(values.Contains(PUBLIC_NOTIFICATION_ID)) {
                model.PublicNotificationId = Convert.ToInt32(values[PUBLIC_NOTIFICATION_ID]);
            }

            if(values.Contains(TITLE)) {
                model.Title = Convert.ToString(values[TITLE]);
            }

            if(values.Contains(BODY)) {
                model.Body = Convert.ToString(values[BODY]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(SLIDER_TYPE_ID)) {
                model.SliderTypeId = values[SLIDER_TYPE_ID] != null ? Convert.ToInt32(values[SLIDER_TYPE_ID]) : (int?)null;
            }

            if(values.Contains(ENTITY_ID)) {
                model.EntityId = values[ENTITY_ID] != null ? Convert.ToInt32(values[ENTITY_ID]) : (int?)null;
            }

            if(values.Contains(ENTITY_NAME_AR)) {
                model.EntityNameAr = Convert.ToString(values[ENTITY_NAME_AR]);
            }

            if(values.Contains(ENTITY_NAME_EN)) {
                model.EntityNameEn = Convert.ToString(values[ENTITY_NAME_EN]);
            }

            if(values.Contains(COUNTRY_ID)) {
                model.CountryId = Convert.ToInt32(values[COUNTRY_ID]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}