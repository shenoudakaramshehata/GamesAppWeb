using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class Platform
    {
        [Key]
        public int PlatformId { get; set; }
    
        public string PlatformTLEN { get; set; }
        public string PlatformTLAR { get; set; }
        public string PlatformPic { get; set; }
        public int OrderIndex { get; set; }

    }
}
