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
    public class ProductImagesController : Controller
    {
        private GamesContext _context;

        public ProductImagesController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var productimages = _context.ProductImages.Select(i => new {
                i.ImageId,
                i.ImageName,
                i.ItemId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ImageId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(productimages, loadOptions));
        }

        [HttpGet]
        public async Task<object> GetImagesForItem([FromQuery] int id)
        {
            var productimages = _context.ProductImages.Where(p => p.ItemId == id).Select(i => new {
                i.ImageId,
                i.ImageName,
                i.ItemId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ImageId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return productimages;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new ProductImages();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.ProductImages.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ImageId });
        }

        [HttpPost]
        public async Task<int> RemoveImageById([FromQuery]int id)
        {
            var itemPic = _context.ProductImages.FirstOrDefault(p => p.ImageId == id);
            _context.ProductImages.Remove(itemPic);
            _context.SaveChanges();

            return id;
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.ProductImages.FirstOrDefaultAsync(item => item.ImageId == key);
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
            var model = await _context.ProductImages.FirstOrDefaultAsync(item => item.ImageId == key);

            _context.ProductImages.Remove(model);
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

        private void PopulateModel(ProductImages model, IDictionary values) {
            string IMAGE_ID = nameof(ProductImages.ImageId);
            string IMAGE_NAME = nameof(ProductImages.ImageName);
            string ITEM_ID = nameof(ProductImages.ItemId);

            if(values.Contains(IMAGE_ID)) {
                model.ImageId = Convert.ToInt32(values[IMAGE_ID]);
            }

            if(values.Contains(IMAGE_NAME)) {
                model.ImageName = Convert.ToString(values[IMAGE_NAME]);
            }

            if(values.Contains(ITEM_ID)) {
                model.ItemId = values[ITEM_ID] != null ? Convert.ToInt32(values[ITEM_ID]) : (int?)null;
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