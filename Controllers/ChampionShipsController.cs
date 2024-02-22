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
    public class ChampionShipsController : Controller
    {
        private GamesContext _context;

        public ChampionShipsController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var championship = _context.ChampionShip.Select(i => new {
                i.ChampionshipId,
                i.ChampionshipName,
                i.ChampionshipPhoto
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ChampionshipId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(championship, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new ChampionShip();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.ChampionShip.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ChampionshipId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int? key, string values) {
            var model = await _context.ChampionShip.FirstOrDefaultAsync(item => item.ChampionshipId == key);
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
        public async Task Delete(int? key) {
            var model = await _context.ChampionShip.FirstOrDefaultAsync(item => item.ChampionshipId == key);

            _context.ChampionShip.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(ChampionShip model, IDictionary values) {
            string CHAMPIONSHIP_ID = nameof(ChampionShip.ChampionshipId);
            string CHAMPIONSHIP_NAME = nameof(ChampionShip.ChampionshipName);
            string CHAMPIONSHIP_PHOTO = nameof(ChampionShip.ChampionshipPhoto);

            if(values.Contains(CHAMPIONSHIP_ID)) {
                model.ChampionshipId = (int)(values[CHAMPIONSHIP_ID] != null ? Convert.ToInt32(values[CHAMPIONSHIP_ID]) : (int?)null);
            }

            if(values.Contains(CHAMPIONSHIP_NAME)) {
                model.ChampionshipName = Convert.ToString(values[CHAMPIONSHIP_NAME]);
            }

            if(values.Contains(CHAMPIONSHIP_PHOTO)) {
                model.ChampionshipPhoto = Convert.ToString(values[CHAMPIONSHIP_PHOTO]);
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