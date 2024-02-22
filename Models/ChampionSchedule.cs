using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class ChampionSchedule
    {
        [Key]
        public int ChampionScheduleId { get; set; }
        public DateTime ChampionScheduleDate { get; set; }
        public int FirstChampionParticipateId { get; set; }
        public int SecoundChampionParticipateId { get; set; }
        public int FirstScore { get; set; }
        public int SecondScore { get; set; }
        public int ChampionId { get; set; }
        [ForeignKey("ChampionId")]
        public Champion champion { get; set; }



    }
}
