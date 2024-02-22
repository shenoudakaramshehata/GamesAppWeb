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
    public class PlatformsController : Controller
    {
        private GamesContext _context;

        public PlatformsController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var platforms = _context.Platforms.Select(i => new {
                i.PlatformId,
                i.PlatformTLEN,
                i.PlatformTLAR,
                i.PlatformPic,
                i.OrderIndex
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PlatformId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(platforms, loadOptions));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllChampionsType(DataSourceLoadOptions loadOptions)
        {
            var championType = _context.ChampionType.Select(i => new {
                i.Id,
                i.NameAr,
                i.NameEn
                
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PlatformId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(championType, loadOptions));
        }


        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Platform();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Platforms.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PlatformId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Platforms.FirstOrDefaultAsync(item => item.PlatformId == key);
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
            var model = await _context.Platforms.FirstOrDefaultAsync(item => item.PlatformId == key);

            _context.Platforms.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Platform model, IDictionary values) {
            string PLATFORM_ID = nameof(Platform.PlatformId);
            string PLATFORM_TLEN = nameof(Platform.PlatformTLEN);
            string PLATFORM_TLAR = nameof(Platform.PlatformTLAR);
            string PLATFORM_PIC = nameof(Platform.PlatformPic);
            string ORDER_INDEX = nameof(Platform.OrderIndex);

            if(values.Contains(PLATFORM_ID)) {
                model.PlatformId = Convert.ToInt32(values[PLATFORM_ID]);
            }

            if(values.Contains(PLATFORM_TLEN)) {
                model.PlatformTLEN = Convert.ToString(values[PLATFORM_TLEN]);
            }

            if(values.Contains(PLATFORM_TLAR)) {
                model.PlatformTLAR = Convert.ToString(values[PLATFORM_TLAR]);
            }

            if(values.Contains(PLATFORM_PIC)) {
                model.PlatformPic = Convert.ToString(values[PLATFORM_PIC]);
            }

            if(values.Contains(ORDER_INDEX)) {
                model.OrderIndex = Convert.ToInt32(values[ORDER_INDEX]);
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