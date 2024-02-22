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
    public class PaymentMehodsController : Controller
    {
        private GamesContext _context;

        public PaymentMehodsController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var paymentmehod = _context.PaymentMehod.Select(i => new {
                i.PaymentMethodId,
                i.PaymentMethodName,
                i.IsActive,
                i.PaymentMethodPic,
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PaymentMethodId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(paymentmehod, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new PaymentMehod();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.PaymentMehod.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PaymentMethodId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.PaymentMehod.FirstOrDefaultAsync(item => item.PaymentMethodId == key);
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
            var model = await _context.PaymentMehod.FirstOrDefaultAsync(item => item.PaymentMethodId == key);

            _context.PaymentMehod.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(PaymentMehod model, IDictionary values) {
            string PAYMENT_METHOD_ID = nameof(PaymentMehod.PaymentMethodId);
            string PAYMENT_METHOD_NAME = nameof(PaymentMehod.PaymentMethodName);
            string IS_ACTIVE = nameof(PaymentMehod.IsActive);

            if(values.Contains(PAYMENT_METHOD_ID)) {
                model.PaymentMethodId = Convert.ToInt32(values[PAYMENT_METHOD_ID]);
            }
            if (values.Contains(IS_ACTIVE))
            {
                model.IsActive = Convert.ToBoolean(values[IS_ACTIVE]);
            }

            if (values.Contains(PAYMENT_METHOD_NAME)) {
                model.PaymentMethodName = Convert.ToString(values[PAYMENT_METHOD_NAME]);
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