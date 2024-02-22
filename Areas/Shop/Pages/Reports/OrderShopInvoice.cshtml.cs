using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.XtraReports.UI;
using Gameapp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gameapp.Areas.Shop.Pages.Reports
{
    public class OrderShopInvoiceModel : PageModel
    {
        private readonly Gameapp.Data.GamesContext _context;
        public OrderShopInvoiceModel(Gameapp.Data.GamesContext context)
        {
            _context = context;
        }



        public XtraReport Report { get; set; }

        public ActionResult OnGet(int id)
        {

            Report = new XtraReport1();

            var order = _context.Order
                .Include(o => o.Customer)
                .Include(o => o.shop)
                .FirstOrDefault(s => s.OrderId == id);

            var orderItems = _context.OrderItems.Include(o => o.Item).Where(s => s.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }


            var dd = _context.OrderItems.Where(o => o.OrderId == id)
                .Select(c => new InvoiceViewModel()
                {
                    Order = order,
                    OrderItem = _context.OrderItems.Include(s => s.Item).FirstOrDefault(s => s.Id == c.Id),
                    InvoiceDate = order.OrderDate.ToString(),
                    InvoiceNo = order.OrderId.ToString(),
                    Customer = order.Customer,
                    Shop = order.shop
                }).ToList();
           

            Report.DataSource = dd;


            return Page();

        }

        public void OnPost()
        {
     

        }
    }
}
