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
 
    public class ChampionParticipatesController : Controller
    {
        private GamesContext _context;

        public ChampionParticipatesController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions,int ChampionId) {
            var championparticipates = _context.ChampionParticipates.Where(e=>e.ChampionId==ChampionId).Select(i => new {
                i.ChampionParticipateId,
                i.ChampionId,
                i.CustomerId,
                i.JoinDate,
                i.ParticipStateId,
                i.Score,
                i.Customer,
                i.Champion
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ChampionParticipateId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(championparticipates, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new ChampionParticipate();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.ChampionParticipates.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ChampionParticipateId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.ChampionParticipates.FirstOrDefaultAsync(item => item.ChampionParticipateId == key);
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
            var model = await _context.ChampionParticipates.FirstOrDefaultAsync(item => item.ChampionParticipateId == key);

            _context.ChampionParticipates.Remove(model);
            await _context.SaveChangesAsync();
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

        [HttpGet]
        public async Task<IActionResult> CustomerLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Customer
                         orderby i.CustomernameAr
                         select new {
                             Value = i.CustomerId,
                             Text = i.CustomernameAr
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ParticipStatesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.ParticipStates
                         orderby i.ParticipStateTlEn
                         select new {
                             Value = i.ParticipStateId,
                             Text = i.ParticipStateTlEn
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(ChampionParticipate model, IDictionary values) {
            string CHAMPION_PARTICIPATE_ID = nameof(ChampionParticipate.ChampionParticipateId);
            string CHAMPION_ID = nameof(ChampionParticipate.ChampionId);
            string CUSTOMER_ID = nameof(ChampionParticipate.CustomerId);
            string JOIN_DATE = nameof(ChampionParticipate.JoinDate);
            string PARTICIP_STATE_ID = nameof(ChampionParticipate.ParticipStateId);
            string SCORE = nameof(ChampionParticipate.Score);

            if(values.Contains(CHAMPION_PARTICIPATE_ID)) {
                model.ChampionParticipateId = Convert.ToInt32(values[CHAMPION_PARTICIPATE_ID]);
            }

            if(values.Contains(CHAMPION_ID)) {
                model.ChampionId = Convert.ToInt32(values[CHAMPION_ID]);
            }

            if(values.Contains(CUSTOMER_ID)) {
                model.CustomerId = Convert.ToInt32(values[CUSTOMER_ID]);
            }

            if(values.Contains(JOIN_DATE)) {
                model.JoinDate = Convert.ToDateTime(values[JOIN_DATE]);
            }

            if(values.Contains(PARTICIP_STATE_ID)) {
                model.ParticipStateId = Convert.ToInt32(values[PARTICIP_STATE_ID]);
            }

            if(values.Contains(SCORE)) {
                model.Score = Convert.ToInt32(values[SCORE]);
            }
        }
        [HttpPut]
        public IActionResult PutAddScore(int key, string values)
        {
            var championParticipate = _context.ChampionParticipates.First(a => a.ChampionParticipateId == key);
            JsonConvert.PopulateObject(values, championParticipate);

           
            _context.SaveChanges();

            return Ok();
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