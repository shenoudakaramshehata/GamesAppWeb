using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models.ViewModels
{
    public class OrderItemsList
    {
        public string ItemName { get; set; }
        public string ShopName { get; set; }
        public string ItemDescription { get; set; }
        public decimal? ItemPrice { get; set; }
        public int? ItemQuantity { get; set; }
        public string ItemImage { get; set; }
    }
}
