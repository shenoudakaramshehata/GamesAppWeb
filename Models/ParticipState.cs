using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class ParticipState
    {
        [Key]
        public int ParticipStateId { get; set; }
        public string ParticipStateTlEn { get; set; }
        public string ParticipStateTlAr { get; set; }

    }
}
