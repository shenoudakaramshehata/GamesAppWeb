using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class Plan
    {
        public int Id { get; set; }
        [Required]
        public string ArabicTitle { get; set; }
        [Required]
        
        public string EnglishTitle { get; set; }
        [Required]

        public int Period { get; set; }
        [Required]
        public int NoOfItems { get; set; }
        public double Price { get; set; }
        public bool VipCollection { get; set; }
        public bool Reports { get; set; }
        public bool Dashboard { get; set; }
        public bool AdzBanners { get; set; }

        public int? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; }
    }
}
