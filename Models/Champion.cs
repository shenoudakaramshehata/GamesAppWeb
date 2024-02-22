using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class Champion
    {
        [Key]
        public int ChampionId { get; set; }
        [Required]

        public string ChampionTlAR { get; set; }
        [Required]

        public string ChampionTlEn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime JoinStart { get; set; }
        public DateTime JoinEnd { get; set; }

        public string ChampionPic { get; set; }
        [Required]
        public string ChampionContent { get; set; }
        public int GameModeId { get; set; }
        [ForeignKey("GameModeId")]
        public GameMode GameMode { get; set; }

        public int ChampTypeId { get; set; }

        [ForeignKey("ChampTypeId")]
        public ChampionType ChampionType { get; set; }
    }
}
