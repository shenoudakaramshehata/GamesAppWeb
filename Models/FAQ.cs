using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class FAQ
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]

        public string Answer { get; set; }
    }
}
