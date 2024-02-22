using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public partial class ContactUS
    {
        [Key]
        public int id { get; set; }

        public string phonenumber1 { get; set; }
        public string phonenumber2 { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }
    }
}
