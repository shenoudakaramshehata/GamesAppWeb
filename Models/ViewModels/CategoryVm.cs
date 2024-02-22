using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models.ViewModels
{
    public class CategoryVm
    {

        public int CategoryId { get; set; }
        [Required]
        public string CategoryTlar { get; set; }
        [Required]
        public string CategoryTlen { get; set; }

        [Required]

        public IFormFile CategoryPic { get; set; }
        [Required]

        public IFormFile CategoryIcon { get; set; }

        public bool? IsActive { get; set; }

        [Required]

        public int? OrderIndex { get; set; }
    }
}
