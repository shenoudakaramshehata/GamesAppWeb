using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models.ViewModels
{
    public class ShopVm
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
        [Required]
        public int? PlanId { get; set; }

        public string Pic { get; set; }
        //public DateTime? RegisterDate { get; set; }
        public int? SubCategoryId { get; set; }

        [Range(0, float.MaxValue)]
        public float? Deliverycost { get; set; }

        [Required]
        //[Remote(action: "CheckEmailExist", controller: "Remote")]
        public string Email { get; set; }
        [Required]
        //[Remote(action: "CheckUserNameExist", controller: "Remote")]

        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
