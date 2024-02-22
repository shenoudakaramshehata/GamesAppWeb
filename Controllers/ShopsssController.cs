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
using Microsoft.AspNetCore.Identity;

namespace Gameapp.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ShopsssController : Controller
    {
        private GamesContext _context;
        private UserManager<ApplicationUser> _userManager;

        public ShopsssController(GamesContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var InstructorID = _userManager.FindByNameAsync(User.Identity.Name).Result.EntityId;

            var shop = _context.Shop.Select(i => new {
                i.ShopId,
                i.ShopTlar,
                i.ShopTlen,
                i.Tele,
                i.Mobile,
                i.Address,
                i.ShopNo,
                i.IsActive,
                i.OrderIndex,
                i.CountryId,
                i.Country,
                i.Banner,
                i.Pic,
                i.RegisterDate,
                i.SubCategoryId,
                i.Deliverycost,
                i.Email,
                i.UserName,
                i.Password
            }).Where(s=>s.ShopId== InstructorID);

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ShopId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(shop, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Shop();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Shop.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ShopId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Shop.FirstOrDefaultAsync(item => item.ShopId == key);
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
            var model = await _context.Shop.FirstOrDefaultAsync(item => item.ShopId == key);

            _context.Shop.Remove(model);
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
        public async Task<IActionResult> SubCategoryLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.SubCategory
                         orderby i.SubCategoryTlar
                         select new {
                             Value = i.SubCategoryId,
                             Text = i.SubCategoryTlar
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Shop model, IDictionary values) {
            string SHOP_ID = nameof(Shop.ShopId);
            string SHOP_TLAR = nameof(Shop.ShopTlar);
            string SHOP_TLEN = nameof(Shop.ShopTlen);
            string TELE = nameof(Shop.Tele);
            string MOBILE = nameof(Shop.Mobile);
            string ADDRESS = nameof(Shop.Address);
            string SHOP_NO = nameof(Shop.ShopNo);
            string IS_ACTIVE = nameof(Shop.IsActive);
            string ORDER_INDEX = nameof(Shop.OrderIndex);
            string COUNTRY_ID = nameof(Shop.CountryId);
            string BANNER = nameof(Shop.Banner);
            string PIC = nameof(Shop.Pic);
            string REGISTER_DATE = nameof(Shop.RegisterDate);
            string SUB_CATEGORY_ID = nameof(Shop.SubCategoryId);
            string DELIVERYCOST = nameof(Shop.Deliverycost);
            string EMAIL = nameof(Shop.Email);
            string USER_NAME = nameof(Shop.UserName);
            string PASSWORD = nameof(Shop.Password);

            if(values.Contains(SHOP_ID)) {
                model.ShopId = Convert.ToInt32(values[SHOP_ID]);
            }

            if(values.Contains(SHOP_TLAR)) {
                model.ShopTlar = Convert.ToString(values[SHOP_TLAR]);
            }

            if(values.Contains(SHOP_TLEN)) {
                model.ShopTlen = Convert.ToString(values[SHOP_TLEN]);
            }

            if(values.Contains(TELE)) {
                model.Tele = Convert.ToString(values[TELE]);
            }

            if(values.Contains(MOBILE)) {
                model.Mobile = Convert.ToString(values[MOBILE]);
            }

            if(values.Contains(ADDRESS)) {
                model.Address = Convert.ToString(values[ADDRESS]);
            }

            if(values.Contains(SHOP_NO)) {
                model.ShopNo = Convert.ToString(values[SHOP_NO]);
            }

            if(values.Contains(IS_ACTIVE)) {
                model.IsActive = values[IS_ACTIVE] != null ? Convert.ToBoolean(values[IS_ACTIVE]) : (bool?)null;
            }

            if(values.Contains(ORDER_INDEX)) {
                model.OrderIndex = values[ORDER_INDEX] != null ? Convert.ToInt32(values[ORDER_INDEX]) : (int?)null;
            }

            if(values.Contains(COUNTRY_ID)) {
                model.CountryId = values[COUNTRY_ID] != null ? Convert.ToInt32(values[COUNTRY_ID]) : (int?)null;
            }

            if(values.Contains(BANNER)) {
                model.Banner = Convert.ToString(values[BANNER]);
            }

            if(values.Contains(PIC)) {
                model.Pic = Convert.ToString(values[PIC]);
            }

            if(values.Contains(REGISTER_DATE)) {
                model.RegisterDate = values[REGISTER_DATE] != null ? Convert.ToDateTime(values[REGISTER_DATE]) : (DateTime?)null;
            }

            if(values.Contains(SUB_CATEGORY_ID)) {
                model.SubCategoryId = values[SUB_CATEGORY_ID] != null ? Convert.ToInt32(values[SUB_CATEGORY_ID]) : (int?)null;
            }

            if(values.Contains(DELIVERYCOST)) {
                model.Deliverycost = values[DELIVERYCOST] != null ? Convert.ToSingle(values[DELIVERYCOST], CultureInfo.InvariantCulture) : (float?)null;
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(USER_NAME)) {
                model.UserName = Convert.ToString(values[USER_NAME]);
            }

            if(values.Contains(PASSWORD)) {
                model.Password = Convert.ToString(values[PASSWORD]);
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