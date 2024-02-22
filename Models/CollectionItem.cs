using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class CollectionItem
    {
        [Key]
        public int CollectionItemId { get; set; }
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Items Items { get; set; }

        public int CollectionId { get; set; }
        [ForeignKey("CollectionId")]
        public Collection Collection { get; set; }
    }
}
