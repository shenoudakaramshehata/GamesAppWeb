using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class PlatformGame
    {
        [Key]
        public int PlatformGameId { get; set; }
        public int PlatFormId { get; set; }
        [ForeignKey("PlatFormId")]
        public Platform platform { get; set; }
        public string GameTLEN { get; set; }
        public string GameTLAR { get; set; }
        public int OrderIndex { get; set; }
        public string GamePic { get; set; }
    }
}
