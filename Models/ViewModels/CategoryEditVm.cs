using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models.ViewModels
{
    public class CategoryEditVm
    {
        public int CategoryId { get; set; }
        [Required]
        public string CategoryTlar { get; set; }
        [Required]
        public string CategoryTlen { get; set; }

        public IFormFile CategoryPic { get; set; }

        public IFormFile CategoryIcon { get; set; }

        public bool? IsActive { get; set; }

        [Required]

        public int? OrderIndex { get; set; }
    }
}
