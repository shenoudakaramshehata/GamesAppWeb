using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class CustomerPlatsGamesFavourite
    {
        [Key]
        public int CustomerFavouriteId { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [ForeignKey("Platform")]
        public int PlatformId { get; set; }

        [ForeignKey("PlatformGame")]

        public int PlatformGameId { get; set; }
        public Platform Platform { get; set; }

        public PlatformGame PlatformGame { get; set; }
    }
}
