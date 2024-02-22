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
    public class ChampionsController : Controller
    {
        private GamesContext _context;

        public ChampionsController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var champions = _context.Champions.Select(i => new {
                i.ChampionId,
                i.ChampionTlAR,
                i.ChampionTlEn,
                i.StartDate,
                i.EndDate,
                i.JoinStart,
                i.JoinEnd,
                i.ChampionPic,
                i.ChampionContent,
                i.GameModeId,
                i.ChampTypeId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ChampionId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(champions, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Champion();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Champions.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ChampionId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Champions.FirstOrDefaultAsync(item => item.ChampionId == key);
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
            var model = await _context.Champions.FirstOrDefaultAsync(item => item.ChampionId == key);

            _context.Champions.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> GameModesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.GameModes
                         orderby i.GameModeTLAR
                         select new {
                             Value = i.GamemodeId,
                             Text = i.GameModeTLAR
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ChampionTypeLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.ChampionType
                         orderby i.NameAr
                         select new {
                             Value = i.Id,
                             Text = i.NameAr
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Champion model, IDictionary values) {
            string CHAMPION_ID = nameof(Champion.ChampionId);
            string CHAMPION_TL_AR = nameof(Champion.ChampionTlAR);
            string CHAMPION_TL_EN = nameof(Champion.ChampionTlEn);
            string START_DATE = nameof(Champion.StartDate);
            string END_DATE = nameof(Champion.EndDate);
            string JOIN_START = nameof(Champion.JoinStart);
            string JOIN_END = nameof(Champion.JoinEnd);
            string CHAMPION_PIC = nameof(Champion.ChampionPic);
            string CHAMPION_CONTENT = nameof(Champion.ChampionContent);
            string GAME_MODE_ID = nameof(Champion.GameModeId);
            string CHAMP_TYPE_ID = nameof(Champion.ChampTypeId);

            if(values.Contains(CHAMPION_ID)) {
                model.ChampionId = Convert.ToInt32(values[CHAMPION_ID]);
            }

            if(values.Contains(CHAMPION_TL_AR)) {
                model.ChampionTlAR = Convert.ToString(values[CHAMPION_TL_AR]);
            }

            if(values.Contains(CHAMPION_TL_EN)) {
                model.ChampionTlEn = Convert.ToString(values[CHAMPION_TL_EN]);
            }

            if(values.Contains(START_DATE)) {
                model.StartDate = Convert.ToDateTime(values[START_DATE]);
            }

            if(values.Contains(END_DATE)) {
                model.EndDate = Convert.ToDateTime(values[END_DATE]);
            }

            if(values.Contains(JOIN_START)) {
                model.JoinStart = Convert.ToDateTime(values[JOIN_START]);
            }

            if(values.Contains(JOIN_END)) {
                model.JoinEnd = Convert.ToDateTime(values[JOIN_END]);
            }

            if(values.Contains(CHAMPION_PIC)) {
                model.ChampionPic = Convert.ToString(values[CHAMPION_PIC]);
            }

            if(values.Contains(CHAMPION_CONTENT)) {
                model.ChampionContent = Convert.ToString(values[CHAMPION_CONTENT]);
            }

            if(values.Contains(GAME_MODE_ID)) {
                model.GameModeId = Convert.ToInt32(values[GAME_MODE_ID]);
            }

            if(values.Contains(CHAMP_TYPE_ID)) {
                model.ChampTypeId = Convert.ToInt32(values[CHAMP_TYPE_ID]);
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