using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class ChampionGroupList
    {
        [Key]
        public int ChampionGroupListId { get; set; }
     
        public int ChampionGroupId { get; set; }
        [ForeignKey("ChampionGroupId")]
        public ChampionGroup ChampionGroup { get; set; }
        public int ChampionId { get; set; }
        [ForeignKey("ChampionId")]
        public Champion champion { get; set; }
    }
}
