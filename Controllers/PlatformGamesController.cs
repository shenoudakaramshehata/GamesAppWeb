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
    public class PlatformGamesController : Controller
    {
        private GamesContext _context;

        public PlatformGamesController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var platformgames = _context.PlatformGames.Select(i => new {
                i.PlatformGameId,
                i.PlatFormId,
                i.GameTLEN,
                i.GameTLAR,
                i.OrderIndex,
                i.GamePic
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PlatformGameId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(platformgames, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new PlatformGame();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.PlatformGames.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PlatformGameId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.PlatformGames.FirstOrDefaultAsync(item => item.PlatformGameId == key);
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
            var model = await _context.PlatformGames.FirstOrDefaultAsync(item => item.PlatformGameId == key);

            _context.PlatformGames.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> PlatformsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Platforms
                         orderby i.PlatformTLEN
                         select new {
                             Value = i.PlatformId,
                             Text = i.PlatformTLEN
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(PlatformGame model, IDictionary values) {
            string PLATFORM_GAME_ID = nameof(PlatformGame.PlatformGameId);
            string PLAT_FORM_ID = nameof(PlatformGame.PlatFormId);
            string GAME_TLEN = nameof(PlatformGame.GameTLEN);
            string GAME_TLAR = nameof(PlatformGame.GameTLAR);
            string ORDER_INDEX = nameof(PlatformGame.OrderIndex);
            string GAME_PIC = nameof(PlatformGame.GamePic);

            if(values.Contains(PLATFORM_GAME_ID)) {
                model.PlatformGameId = Convert.ToInt32(values[PLATFORM_GAME_ID]);
            }

            if(values.Contains(PLAT_FORM_ID)) {
                model.PlatFormId = Convert.ToInt32(values[PLAT_FORM_ID]);
            }

            if(values.Contains(GAME_TLEN)) {
                model.GameTLEN = Convert.ToString(values[GAME_TLEN]);
            }

            if(values.Contains(GAME_TLAR)) {
                model.GameTLAR = Convert.ToString(values[GAME_TLAR]);
            }

            if(values.Contains(ORDER_INDEX)) {
                model.OrderIndex = Convert.ToInt32(values[ORDER_INDEX]);
            }

            if(values.Contains(GAME_PIC)) {
                model.GamePic = Convert.ToString(values[GAME_PIC]);
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