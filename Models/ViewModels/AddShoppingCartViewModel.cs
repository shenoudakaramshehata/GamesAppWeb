using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models.ViewModels
{
    public class AddShoppingCartViewModel
    {
        //int CustomerId,int ItemId,decimal price,int QTY,float Total
        public int CustomerId { get; set; }
        public int ItemId { get; set; }
        public float ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public float ItemTotal { get; set; }
    }
}
