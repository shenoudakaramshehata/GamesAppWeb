using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class ChampionGroup
    {
        [Key]
        public int ChampionGroupID { get; set; }
        public string ChampionGroupTlAr { get; set; }
        public string ChampionGroupEn { get; set; }

    }
}
