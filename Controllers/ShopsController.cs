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
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ShopsController : Controller
    {
        private GamesContext _context;

        public ShopsController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var shop = _context.Shop.Include(s => s.Country).Select(i => new {
                i.ShopId,
                //i.CategoryId,
                i.ShopTlar,
                i.ShopTlen,
                i.Tele,
                i.Mobile,
                i.Address,
                i.ShopNo,
                i.IsActive,
                i.OrderIndex,
                i.Pic,
                i.RegisterDate,
                i.Country
            });

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
        public async Task<IActionResult> CategoryLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Category
                         orderby i.CategoryTlar
                         select new {
                             Value = i.CategoryId,
                             Text = i.CategoryTlar
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> CountryLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Country
                         orderby i.CountryTlar
                         select new
                         {
                             Value = i.CountryId,
                             Text = i.CountryTlar
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }


        //[HttpGet]
        //public async Task<IActionResult> CountryLookup(DataSourceLoadOptions loadOptions)
        //{
        //    var lookup = from i in _context.Country
        //                 orderby i.CountryId
        //                 select new
        //                 {
        //                     Value = i.CountryId,
        //                     Text = i.CountryTlen
        //                 };
        //    return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        //}
        private void PopulateModel(Shop model, IDictionary values) {
            string SHOP_ID = nameof(Shop.ShopId);
            //string CATEGORY_ID = nameof(Shop.CategoryId);
            string SHOP_TLAR = nameof(Shop.ShopTlar);
            string SHOP_TLEN = nameof(Shop.ShopTlen);
            string TELE = nameof(Shop.Tele);
            string MOBILE = nameof(Shop.Mobile);
            string ADDRESS = nameof(Shop.Address);
            string SHOP_NO = nameof(Shop.ShopNo);
            string IS_ACTIVE = nameof(Shop.IsActive);
            string ORDER_INDEX = nameof(Shop.OrderIndex);
            string PIC = nameof(Shop.Pic);
            string REGISTER_DATE = nameof(Shop.RegisterDate);

            if(values.Contains(SHOP_ID)) {
                model.ShopId = Convert.ToInt32(values[SHOP_ID]);
            }

            //if(values.Contains(CATEGORY_ID)) {
            //    model.CategoryId = values[CATEGORY_ID] != null ? Convert.ToInt32(values[CATEGORY_ID]) : (int?)null;
            //}

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

            if(values.Contains(PIC)) {
                model.Pic = Convert.ToString(values[PIC]);
            }

            if(values.Contains(REGISTER_DATE)) {
                model.RegisterDate = values[REGISTER_DATE] != null ? Convert.ToDateTime(values[REGISTER_DATE]) : (DateTime?)null;
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