using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class GameMode
    {
        [Key]
        public int GamemodeId { get; set; }
        public string GameModeTLAR { get; set; }
        public string GameModeTLEn { get; set; }

    }
}
