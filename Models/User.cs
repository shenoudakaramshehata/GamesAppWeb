using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Gameapp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int EntityId { get; set; }
        public string Pic { get; set; }

    }
}
