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
    public class SlidersController : Controller
    {
        private Gameapp.Data.GamesContext _context;

        public SlidersController(Gameapp.Data.GamesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var slider = _context.Slider.Include(s => s.Country).Select(i => new
            {
                i.SliderId,
                i.Pic,
                i.IsActive,
                i.SliderTypeId,
                i.EntityId,
                i.OrderIndex,
                i.Country
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "SliderId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(slider, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new Slider();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Slider.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.SliderId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var model = await _context.Slider.FirstOrDefaultAsync(item => item.SliderId == key);
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
            var model = await _context.Slider.FirstOrDefaultAsync(item => item.SliderId == key);

            _context.Slider.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> SliderTypeLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.SliderType
                         orderby i.SliderTypeTlar
                         select new
                         {
                             Value = i.SliderTypeId,
                             Text = i.SliderTypeTlar
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }


        [HttpGet]
        public async Task<IActionResult> EntityIdLookup(DataSourceLoadOptions loadOptions)
        {

            int? sliderTypeId = null;

            if (loadOptions.Filter != null)
            {
                sliderTypeId = int.Parse(loadOptions.Filter[2].ToString());
            }

            if (sliderTypeId == 1)
            {
                var lookup = from i in _context.Items
                             orderby i.ItemId
                             select new
                             {
                                 ID = i.ItemId,
                                 Name = i.ItemName,
                                 SliderTypeId = 1
                             };
                return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
            }

            else if (sliderTypeId == 2)
            {
                var lookup = from i in _context.Shop
                             orderby i.ShopId
                             select new
                             {
                                 ID = i.ShopId,
                                 Name = i.ShopTlen,
                                 SliderTypeId = 2
                             };
                return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
            }

            else if (sliderTypeId == 3)
            {
                var lookup = from i in _context.Champions
                             orderby i.ChampionId
                             select new
                             {
                                 ID = i.ChampionId,
                                 Name = i.ChampionTlEn,
                                 SliderTypeId = 3
                             };
                return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
            }
            else if (sliderTypeId == 4)
            {
                var lookup = from i in _context.Items
                             orderby i.ItemId

                             select new
                             {
                                 ID = i.ItemId,
                                 Name = i.ItemName,
                                 SliderTypeId = 4
                             };
                return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
            }

            else if (sliderTypeId == 5)
            {
                var lookup = from i in _context.Items
                             orderby i.ItemId
                             select new
                             {
                                 ID = i.ItemId,
                                 Name = i.ItemName,
                                 SliderTypeId = 5
                             };
                return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
            }

            else
            {
                var lookup = from i in _context.Banner
                             orderby i.BannerId

                             select new
                             {
                                 ID = i.EntityId,
                                 SliderTypeId = i.SliderTypeId
                             };

                //string getEntityName(string entityId, int? sliderTypeId)
                //{
                //    if (entityId == "" || entityId == null)
                //    {
                //        return "";
                //    }

                //    if (sliderTypeId == 1)
                //    {
                //        return _context.Items.FirstOrDefault(s => s.ItemId == int.Parse(entityId)).ItemName;
                        
                //    }

                //    else if (sliderTypeId == 2)
                //    {
                //        return _context.Shop.FirstOrDefault(s => s.ShopId == int.Parse(entityId)).ShopTlen;

                //    }

                //    else if (sliderTypeId == 3)
                //    {
                //        return _context.Champions.FirstOrDefault(s => s.ChampionId == int.Parse(entityId)).ChampionTlEn;

                //    }

                //    return "";
                //}

                //var defaultLookup = lookup.AsEnumerable().Select(i => new
                //{
                //    ID = i.ID,
                //    Name = getEntityName(i.ID, i.SliderTypeId)

                //}).ToList();

                return Json( DataSourceLoader.Load(lookup, loadOptions));
            }

        }

        private void PopulateModel(Slider model, IDictionary values)
        {
            string SLIDER_ID = nameof(Slider.SliderId);
            string PIC = nameof(Slider.Pic);
            string IS_ACTIVE = nameof(Slider.IsActive);
            string SLIDER_TYPE_ID = nameof(Slider.SliderTypeId);
            string ENTITY_ID = nameof(Slider.EntityId);
            string ORDER_INDEX = nameof(Slider.OrderIndex);

            if (values.Contains(SLIDER_ID))
            {
                model.SliderId = Convert.ToInt32(values[SLIDER_ID]);
            }

            if (values.Contains(PIC))
            {
                model.Pic = Convert.ToString(values[PIC]);
            }

            if (values.Contains(IS_ACTIVE))
            {
                model.IsActive = values[IS_ACTIVE] != null ? Convert.ToBoolean(values[IS_ACTIVE]) : (bool?)null;
            }

            if (values.Contains(SLIDER_TYPE_ID))
            {
                model.SliderTypeId = values[SLIDER_TYPE_ID] != null ? Convert.ToInt32(values[SLIDER_TYPE_ID]) : (int?)null;
            }

            if (values.Contains(ENTITY_ID))
            {
                model.EntityId = Convert.ToString(values[ENTITY_ID]);
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