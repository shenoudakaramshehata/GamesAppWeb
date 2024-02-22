using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class Collection
    {
        [Key]
        public int CollectionId { get; set; }
        [Required]
        public string CollectionTitleAr { get; set; }
        [Required]
        public string CollectionTitleEn { get; set; }

        [Required]
        public int Source { get; set; }
        public bool IsActive { get; set; }
    }
}
