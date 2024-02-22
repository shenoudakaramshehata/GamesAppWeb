using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public partial class ProductImages
    {
        [Key]
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        [Column("ItemID")]
        public int? ItemId { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Items Item { get; set; }
    }
}
