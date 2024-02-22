using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models.ViewModels
{
    public class AddPublicShop
    {
        public int ShopId { get; set; }
        [StringLength(50)]
        [Required]
        public string ShopTlar { get; set; }
        [StringLength(50)]
        [Required]

        public string ShopTlen { get; set; }
        [StringLength(50)]
        [Required]

        public string Tele { get; set; }
        [StringLength(50)]
        [Required]

        public string Mobile { get; set; }
        [StringLength(50)]
        [Required]


        public string Address { get; set; }
        [StringLength(50)]

        public string ShopNo { get; set; }
        public bool? IsActive { get; set; }

        [Required]
        public int? OrderIndex { get; set; }

        [Required]
        public int? CountryId { get; set; }

        public string Pic { get; set; }
        public string Banner { get; set; }
        public DateTime? RegisterDate { get; set; }
        public int? SubCategoryId { get; set; }

        [Range(0, float.MaxValue)]
        public float? Deliverycost { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]

        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public IFormFile BannerFormFile { get; set; }
        public IFormFile PicFormFile { get; set; }

    }
}
