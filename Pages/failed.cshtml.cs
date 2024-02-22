using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Gameapp.Data;
using Gameapp.Models;

namespace Gameapp.Pages
{
    public class failedModel : PageModel
    {
        private GamesContext _context;
        public List<Order> order { get; set; }

        private readonly IEmailSender _emailSender;

        public failedModel(GamesContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        public void OnGet(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        string Ref, string TrackID, string Auth)
        {
            if (OrderID != 0)
            {
                order = _context.Order.Where(e => e.uniqeId == OrderID).ToList();
                _context.Order.RemoveRange(order);
                _context.SaveChanges();
            }
        }
    }
}
