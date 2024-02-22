using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class ChampionParticipate
    {
        [Key]
        public int ChampionParticipateId { get; set; }
        public int ChampionId { get; set; }
        [ForeignKey("ChampionId")]
        public Champion Champion { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public DateTime JoinDate { get; set; }

        [ForeignKey("ParticipState")]
        public int ParticipStateId { get; set; }

        [ForeignKey("ParticipStateId")]

        public ParticipState ParticipState { get; set; }

        public int? Score { get; set; }
    }
}
