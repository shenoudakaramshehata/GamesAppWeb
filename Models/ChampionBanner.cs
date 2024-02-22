using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class ChampionBanner
    {
        [Key]
        public int ChampionBannerID { get; set; }
        public string ChampionBannerPic { get; set; }
    }
}
