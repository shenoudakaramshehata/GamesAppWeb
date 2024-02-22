using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public Items Item { get; set; }
        public float ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public float Total { get; set; }
    }
}
