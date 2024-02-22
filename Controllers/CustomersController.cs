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
    public class CustomersController : Controller
    {
        private GamesContext _context;

        public CustomersController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var customer = _context.Customer.Select(i => new {
                i.CustomerId,
                i.CustomernameAr,
                i.CustomernameEn,
                CustomerAddress= _context.ShippingAddress.Where(e => e.CustomerId == i.CustomerId).FirstOrDefault()!=null?_context.ShippingAddress.Where(e=>e.CustomerId==i.CustomerId).FirstOrDefault().AddressNickname+" ,"+ _context.ShippingAddress.Where(e => e.CustomerId == i.CustomerId).FirstOrDefault().Area
                +" ," + _context.ShippingAddress.Where(e => e.CustomerId == i.CustomerId).FirstOrDefault().Street:null,
                i.Customerphone,
                i.CustomerEmail,
                i.FavouriteItem
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CustomerId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(customer, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Customer();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Customer.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CustomerId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Customer.FirstOrDefaultAsync(item => item.CustomerId == key);
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
            var model = await _context.Customer.FirstOrDefaultAsync(item => item.CustomerId == key);

            _context.Customer.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Customer model, IDictionary values) {
            string CUSTOMER_ID = nameof(Customer.CustomerId);
            string CUSTOMERNAME_AR = nameof(Customer.CustomernameAr);
            string CUSTOMERNAME_EN = nameof(Customer.CustomernameEn);
            string CUSTOMER_ADDRESS = nameof(Customer.CustomerAddress);
            string CUSTOMERPHONE = nameof(Customer.Customerphone);
            string CUSTOMER_EMAIL = nameof(Customer.CustomerEmail);
            string FAVOURITE_ITEM = nameof(Customer.FavouriteItem);

            if(values.Contains(CUSTOMER_ID)) {
                model.CustomerId = Convert.ToInt32(values[CUSTOMER_ID]);
            }

            if(values.Contains(CUSTOMERNAME_AR)) {
                model.CustomernameAr = Convert.ToString(values[CUSTOMERNAME_AR]);
            }

            if(values.Contains(CUSTOMERNAME_EN)) {
                model.CustomernameEn = Convert.ToString(values[CUSTOMERNAME_EN]);
            }

            if(values.Contains(CUSTOMER_ADDRESS)) {
                model.CustomerAddress = Convert.ToString(values[CUSTOMER_ADDRESS]);
            }

            if(values.Contains(CUSTOMERPHONE)) {
                model.Customerphone = Convert.ToString(values[CUSTOMERPHONE]);
            }

            if(values.Contains(CUSTOMER_EMAIL)) {
                model.CustomerEmail = Convert.ToString(values[CUSTOMER_EMAIL]);
            }

            if(values.Contains(FAVOURITE_ITEM)) {
                model.FavouriteItem = Convert.ToString(values[FAVOURITE_ITEM]);
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