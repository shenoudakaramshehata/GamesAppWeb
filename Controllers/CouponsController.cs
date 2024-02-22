﻿using DevExtreme.AspNet.Data;
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
    public class CouponsController : Controller
    {
        private GamesContext _context;

        public CouponsController(GamesContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var coupon = _context.Coupon.Select(i => new {
                i.Id,
                i.Serial,
                i.ExpirationDate,
                i.IssueDate,
                i.Amount,
                i.CouponTypeId,
                i.Used
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(coupon, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Coupon();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Coupon.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Coupon.FirstOrDefaultAsync(item => item.Id == key);
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
        public async Task<IActionResult> Delete(int key) {
            var model = await _context.Coupon.FirstOrDefaultAsync(item => item.Id == key);

            if (_context.Order.Where(o => o.CouponId == model.Id).Count() > 0 )
            {
                return StatusCode(409, "You cannot delete this coupon because it used in orders");
            }


            _context.Coupon.Remove(model);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> CouponTypeLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.CouponType
                         orderby i.Title
                         select new {
                             Value = i.Id,
                             Text = i.Title
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Coupon model, IDictionary values) {
            string ID = nameof(Coupon.Id);
            string SERIAL = nameof(Coupon.Serial);
            string EXPIRATION_DATE = nameof(Coupon.ExpirationDate);
            string ISSUE_DATE = nameof(Coupon.IssueDate);
            string AMOUNT = nameof(Coupon.Amount);
            string COUPON_TYPE_ID = nameof(Coupon.CouponTypeId);
            string USED = nameof(Coupon.Used);

            if(values.Contains(ID)) {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(SERIAL)) {
                model.Serial = Convert.ToString(values[SERIAL]);
            }

            if(values.Contains(EXPIRATION_DATE)) {
                model.ExpirationDate = Convert.ToDateTime(values[EXPIRATION_DATE]);
            }

            if(values.Contains(ISSUE_DATE)) {
                model.IssueDate = Convert.ToDateTime(values[ISSUE_DATE]);
            }

            if(values.Contains(AMOUNT)) {
                model.Amount = values[AMOUNT] != null ? Convert.ToDouble(values[AMOUNT], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(COUPON_TYPE_ID)) {
                model.CouponTypeId = Convert.ToInt32(values[COUPON_TYPE_ID]);
            }

            if(values.Contains(USED)) {
                model.Used = Convert.ToBoolean(values[USED]);
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