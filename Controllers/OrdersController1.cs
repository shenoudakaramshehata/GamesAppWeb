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
    public class OrdersController1Controller : Controller
    {
        private GamesContext _context;

        public OrdersController1Controller(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var order = _context.Order.Select(i => new {
                i.OrderId,
                i.CustomerId,
                i.ShippingAddressId,
                i.IsDeliverd,
                i.Notes,
                i.OrderIndex,
                i.PaymentMehodPaymentMethodId,
                i.Deliverycost,
                i.OrderDiscount,
                i.ItemsItemId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "OrderId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(order, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Order();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Order.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.OrderId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Order.FirstOrDefaultAsync(item => item.OrderId == key);
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
            var model = await _context.Order.FirstOrDefaultAsync(item => item.OrderId == key);

            _context.Order.Remove(model);
            await _context.SaveChangesAsync();
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
        public async Task<IActionResult> ItemsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Items
                         orderby i.ItemName
                         select new {
                             Value = i.ItemId,
                             Text = i.ItemName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> PaymentMehodLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.PaymentMehod
                         orderby i.PaymentMethodName
                         select new {
                             Value = i.PaymentMethodId,
                             Text = i.PaymentMethodName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ShippingAddressLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.ShippingAddress
                         orderby i.ShippingAddressId
                         select new
                         {
                             Value = i.ShippingAddressId,
                             Text = i.AddressNickname
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ShopLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Shop
                         orderby i.ShopTlar
                         select new {
                             Value = i.ShopId,
                             Text = i.ShopTlar
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Order model, IDictionary values) {
            string ORDER_ID = nameof(Order.OrderId);
            string CUSTOMER_ID = nameof(Order.CustomerId);
            string SHIPPING_ADDRESS_ID = nameof(Order.ShippingAddressId);
            string IS_DELIVERD = nameof(Order.IsDeliverd);
            string NOTES = nameof(Order.Notes);
            string ORDER_INDEX = nameof(Order.OrderIndex);
            string PAYMENT_MEHOD_PAYMENT_METHOD_ID = nameof(Order.PaymentMehodPaymentMethodId);
            string DELIVERYCOST = nameof(Order.Deliverycost);
            string DISSCOUNT = nameof(Order.OrderDiscount);
            string ITEMS_ITEM_ID = nameof(Order.ItemsItemId);

            if(values.Contains(ORDER_ID)) {
                model.OrderId = Convert.ToInt32(values[ORDER_ID]);
            }

            if(values.Contains(CUSTOMER_ID)) {
                model.CustomerId = values[CUSTOMER_ID] != null ? Convert.ToInt32(values[CUSTOMER_ID]) : (int?)null;
            }

            if(values.Contains(SHIPPING_ADDRESS_ID)) {
                model.ShippingAddressId = values[SHIPPING_ADDRESS_ID] != null ? Convert.ToInt32(values[SHIPPING_ADDRESS_ID]) : (int?)null;
            }

            if(values.Contains(IS_DELIVERD)) {
                model.IsDeliverd = values[IS_DELIVERD] != null ? Convert.ToBoolean(values[IS_DELIVERD]) : (bool?)null;
            }

            if(values.Contains(NOTES)) {
                model.Notes = Convert.ToString(values[NOTES]);
            }

            if(values.Contains(ORDER_INDEX)) {
                model.OrderIndex = values[ORDER_INDEX] != null ? Convert.ToInt32(values[ORDER_INDEX]) : (int?)null;
            }

            if(values.Contains(PAYMENT_MEHOD_PAYMENT_METHOD_ID)) {
                model.PaymentMehodPaymentMethodId = values[PAYMENT_MEHOD_PAYMENT_METHOD_ID] != null ? Convert.ToInt32(values[PAYMENT_MEHOD_PAYMENT_METHOD_ID]) : (int?)null;
            }

            if(values.Contains(DELIVERYCOST)) {
                model.Deliverycost = Convert.ToInt32(values[DELIVERYCOST]);
            }

            if(values.Contains(DISSCOUNT)) {
                model.OrderDiscount = Convert.ToInt32(values[DISSCOUNT]);
            }

            if(values.Contains(ITEMS_ITEM_ID)) {
                model.ItemsItemId = values[ITEMS_ITEM_ID] != null ? Convert.ToInt32(values[ITEMS_ITEM_ID]) : (int?)null;
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