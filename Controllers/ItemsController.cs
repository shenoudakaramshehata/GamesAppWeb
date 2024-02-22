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
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ItemsController : Controller
    {
        private GamesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemsController(GamesContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var items = _context.Items.Select(i => new {
                i.ItemId,
                i.CategoryId,
                i.SubCategoryId,
                i.ShopId,
                i.ItemName,
                i.ItemNameAr,
                i.ItemImage,
                i.ItemDescription,
                i.ItemDescriptionAr,
                i.ItemPrice,
                i.IsActive,
                i.OrderIndex,
                i.CustomerId,
                i.IsFavourite,
                i.OutOfStock,
                i.CountryId
            });

            return Json(await DataSourceLoader.LoadAsync(items, loadOptions));
        }
        [HttpGet]
        public async Task<IActionResult> GetItemsOfShopHasCollection(DataSourceLoadOptions loadOptions)
        {
            var SubscriptionsList =
                                     _context.Subscriptions
                                        .Include(q => q.Plan).Where(e => e.Plan.VipCollection == true)
                                    .OrderBy(i => i.Id).ToList();
            List<Items> items1 = new List<Items>();
    
            var plans = _context.Plans.Where(e => e.VipCollection == true);
            var items = _context.Items.Select(i => new {
                i.ItemId,
                i.CategoryId,
                i.SubCategoryId,
                i.ShopId,
                i.ItemName,
                i.ItemNameAr,
                i.ItemImage,
                i.ItemDescription,
                i.ItemDescriptionAr,
                i.ItemPrice,
                i.IsActive,
                i.OrderIndex,
                i.CustomerId,
                i.IsFavourite,
                i.OutOfStock,
                i.CountryId
            });

            return Json(await DataSourceLoader.LoadAsync(items, loadOptions));
        }
        [HttpGet]
        public async Task<IActionResult> GetByShopId(DataSourceLoadOptions loadOptions, string id)
        {
            var items = _context.Items
                .Where(s => s.ShopId == int.Parse(id))
                .Select(i => new
                {
                    i.ItemId,
                    i.CategoryId,
                    i.SubCategoryId,
                    i.ShopId,
                    i.ItemName,
                    i.ItemNameAr,
                    i.ItemImage,
                    i.ItemDescription,
                    i.ItemDescriptionAr,
                    i.ItemPrice,
                    i.IsActive,
                    i.OrderIndex,
                    i.CustomerId,
                    i.IsFavourite,
                    i.OutOfStock,
                    i.CountryId
                });
            return Json(await DataSourceLoader.LoadAsync(items, loadOptions));

        }
        [HttpGet]
        public async Task<IActionResult> GetItemsInSpecificCollection(DataSourceLoadOptions loadOptions, string id)
        {
            var items = _context.CollectionItems.Include(e=>e.Items)
                .Where(s => s.CollectionId == int.Parse(id))
                .Select(i => new
                {
                    i.ItemId,
                    i.Items.CategoryId,
                    i.Items.SubCategoryId,
                    i.Items.ShopId,
                    i.Items.ItemName,
                    i.Items.ItemNameAr,
                    i.Items.ItemImage,
                    i.Items.ItemDescription,
                    i.Items.ItemDescriptionAr,
                    i.Items.ItemPrice,
                    i.Items.IsActive,
                    i.Items.OrderIndex,
                    i.Items.CustomerId,
                    i.Items.IsFavourite,
                    i.Items.OutOfStock,
                    i.Items.CountryId
                });
            return Json(await DataSourceLoader.LoadAsync(items, loadOptions));

        }
        [HttpGet]
        public async Task<IActionResult> GetItemsForSpecificShop(DataSourceLoadOptions loadOptions)
        {
            var shopId = _userManager.FindByNameAsync(User.Identity.Name).Result.EntityId;

            var items = _context.Items.Where(i => i.ShopId == shopId);

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ItemId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(items, loadOptions));
        }


        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new Items();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Items.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ItemId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var model = await _context.Items.FirstOrDefaultAsync(item => item.ItemId == key);
            if (model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key)
        {
            var model = await _context.Items.FirstOrDefaultAsync(item => item.ItemId == key);

            _context.Items.Remove(model);
            await _context.SaveChangesAsync();
        }



        [HttpGet]
        public async Task<IActionResult> CategoryLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Category
                         orderby i.CategoryTlar
                         select new
                         {
                             Value = i.CategoryId,
                             Text = i.CategoryTlar
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ShopLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Shop
                         orderby i.ShopTlar
                         select new
                         {
                             Value = i.ShopId,
                             Text = i.ShopTlar
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> SubCategoryLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.SubCategory.OrderBy(c => c.OrderIndex);

            //var lookup = from i in _context.SubCategory
            //             orderby i.SubCategoryTlar
            //             select new {
            //                 Value = i.SubCategoryId,
            //                 Text = i.SubCategoryTlar
            //             };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }
        [HttpGet]
        public async Task<IActionResult> PlansLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.Plans.OrderBy(c => c.Id);

            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> SubCategoryLookup2(DataSourceLoadOptions loadOptions)
        {
            //var lookup = _context.SubCategory.OrderBy(c => c.OrderIndex);

            var lookup = from i in _context.SubCategory
                         orderby i.SubCategoryTlar
                         select new
                         {
                             Value = i.SubCategoryId,
                             Text = i.SubCategoryTlar
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> SubCategoryLookupForEdit(DataSourceLoadOptions loadOptions)
        {

            var lookup = from i in _context.SubCategory
                         orderby i.SubCategoryTlar
                         select new
                         {
                             Value = i.SubCategoryId,
                             Text = i.SubCategoryTlar
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }
        private void PopulateModel(Items model, IDictionary values)
        {
            string ITEM_ID = nameof(Items.ItemId);
            string CATEGORY_ID = nameof(Items.CategoryId);
            string SUB_CATEGORY_ID = nameof(Items.SubCategoryId);
            string SHOP_ID = nameof(Items.ShopId);
            string ITEM_NAME = nameof(Items.ItemName);
            string ITEM_IMAGE = nameof(Items.ItemImage);
            string ITEM_DESCRIPTION = nameof(Items.ItemDescription);
            string ITEM_PRICE = nameof(Items.ItemPrice);
            string IS_ACTIVE = nameof(Items.IsActive);
            string ORDER_INDEX = nameof(Items.OrderIndex);

            if (values.Contains(ITEM_ID))
            {
                model.ItemId = Convert.ToInt32(values[ITEM_ID]);
            }

            if (values.Contains(CATEGORY_ID))
            {
                model.CategoryId = values[CATEGORY_ID] != null ? Convert.ToInt32(values[CATEGORY_ID]) : (int?)null;
            }

            if (values.Contains(SUB_CATEGORY_ID))
            {
                model.SubCategoryId = values[SUB_CATEGORY_ID] != null ? Convert.ToInt32(values[SUB_CATEGORY_ID]) : (int?)null;
            }

            if (values.Contains(SHOP_ID))
            {
                model.ShopId = values[SHOP_ID] != null ? Convert.ToInt32(values[SHOP_ID]) : (int?)null;
            }

            if (values.Contains(ITEM_NAME))
            {
                model.ItemName = Convert.ToString(values[ITEM_NAME]);
            }

            if (values.Contains(ITEM_IMAGE))
            {
                model.ItemImage = Convert.ToString(values[ITEM_IMAGE]);
            }

            if (values.Contains(ITEM_DESCRIPTION))
            {
                model.ItemDescription = Convert.ToString(values[ITEM_DESCRIPTION]);
            }

            if (values.Contains(ITEM_PRICE))
            {
                model.ItemPrice = values[ITEM_PRICE] != null ? Convert.ToDecimal(values[ITEM_PRICE], CultureInfo.InvariantCulture) : (decimal?)null;
            }

            //if(values.Contains(IS_FAVOURITE)) {
            //    model.IsFavourite = values[IS_FAVOURITE] != null ? Convert.ToBoolean(values[IS_FAVOURITE]) : (bool?)null;
            //}

            if (values.Contains(IS_ACTIVE))
            {
                model.IsActive = values[IS_ACTIVE] != null ? Convert.ToBoolean(values[IS_ACTIVE]) : (bool?)null;
            }

            if (values.Contains(ORDER_INDEX))
            {
                model.OrderIndex = values[ORDER_INDEX] != null ? Convert.ToInt32(values[ORDER_INDEX]) : (int?)null;
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}