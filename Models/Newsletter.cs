using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class Newsletter
    {
        public int NewsletterID { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
    }
}
