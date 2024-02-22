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
    public class BestOffersController : Controller
    {
        private GamesContext _context;

        public BestOffersController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var bestoffer = _context.BestOffer.Select(i => new {
                i.BestOfferId,
                i.ItemId,
                i.ItemName,
                i.ItemPhoto,
                i.ItemDescription,
                i.ItemPrice,
                i.ShopName,
                
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "BestOfferId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(bestoffer, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new BestOffer();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.BestOffer.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.BestOfferId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int? key, string values) {
            var model = await _context.BestOffer.FirstOrDefaultAsync(item => item.BestOfferId == key);
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
            var model = await _context.BestOffer.FirstOrDefaultAsync(item => item.BestOfferId == key);

            _context.BestOffer.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> ItemsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Items
                         orderby i.ItemName
                         select new {
                             Value = i.ItemId,
                             Text = i.ItemName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(BestOffer model, IDictionary values) {
            string BEST_OFFER_ID = nameof(BestOffer.BestOfferId);
            string ITEM_ID = nameof(BestOffer.ItemId);
            string ITEM_NAME = nameof(BestOffer.ItemName);
            string ITEM_PHOTO = nameof(BestOffer.ItemPhoto);
            string ITEM_DESCRIPTION = nameof(BestOffer.ItemDescription);
            string ITEM_PRICE = nameof(BestOffer.ItemPrice);
            string SHOP_NAME = nameof(BestOffer.ShopName);
           

            if(values.Contains(BEST_OFFER_ID)) {
                model.BestOfferId = (int)(values[BEST_OFFER_ID] != null ? Convert.ToInt32(values[BEST_OFFER_ID]) : (int?)null);
            }

            if(values.Contains(ITEM_ID)) {
                model.ItemId = values[ITEM_ID] != null ? Convert.ToInt32(values[ITEM_ID]) : (int?)null;
            }

            if(values.Contains(ITEM_NAME)) {
                model.ItemName = Convert.ToString(values[ITEM_NAME]);
            }

            if(values.Contains(ITEM_PHOTO)) {
                model.ItemPhoto = Convert.ToString(values[ITEM_PHOTO]);
            }

            if(values.Contains(ITEM_DESCRIPTION)) {
                model.ItemDescription = Convert.ToString(values[ITEM_DESCRIPTION]);
            }

            if(values.Contains(ITEM_PRICE)) {
                model.ItemPrice = values[ITEM_PRICE] != null ? Convert.ToInt32(values[ITEM_PRICE]) : (int?)null;
            }

            if(values.Contains(SHOP_NAME)) {
                model.ShopName = Convert.ToString(values[SHOP_NAME]);
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