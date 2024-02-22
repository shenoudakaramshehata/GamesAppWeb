using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public int CustomerId { get; set; }
    }
}
