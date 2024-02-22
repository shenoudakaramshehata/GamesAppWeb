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
    public class BannersController : Controller
    {
        private GamesContext _context;

        public BannersController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var banner = _context.Banner.Include(b => b.Country).Select(i => new {
                i.BannerId,
                i.Pic,
                i.IsActive,
                i.OrderIndex,
                i.SliderTypeId,
                i.EntityId,
                i.Country
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "BannerId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(banner, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Banner();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Banner.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.BannerId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Banner.FirstOrDefaultAsync(item => item.BannerId == key);
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
            var model = await _context.Banner.FirstOrDefaultAsync(item => item.BannerId == key);

            _context.Banner.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Banner model, IDictionary values) {
            string BANNER_ID = nameof(Banner.BannerId);
            string PIC = nameof(Banner.Pic);
            string IS_ACTIVE = nameof(Banner.IsActive);
            string ORDER_INDEX = nameof(Banner.OrderIndex);
            string SLIDER_TYPE_ID = nameof(Banner.SliderTypeId);
            string ENTITY_ID = nameof(Banner.EntityId);

            if(values.Contains(BANNER_ID)) {
                model.BannerId = Convert.ToInt32(values[BANNER_ID]);
            }

            if(values.Contains(PIC)) {
                model.Pic = Convert.ToString(values[PIC]);
            }

            if(values.Contains(IS_ACTIVE)) {
                model.IsActive = values[IS_ACTIVE] != null ? Convert.ToBoolean(values[IS_ACTIVE]) : (bool?)null;
            }

            if(values.Contains(ORDER_INDEX)) {
                model.OrderIndex = values[ORDER_INDEX] != null ? Convert.ToInt32(values[ORDER_INDEX]) : (int?)null;
            }

            if(values.Contains(SLIDER_TYPE_ID)) {
                model.SliderTypeId = values[SLIDER_TYPE_ID] != null ? Convert.ToInt32(values[SLIDER_TYPE_ID]) : (int?)null;
            }

            if(values.Contains(ENTITY_ID)) {
                model.EntityId = Convert.ToString(values[ENTITY_ID]);
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