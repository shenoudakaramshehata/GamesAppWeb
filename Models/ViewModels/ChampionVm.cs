using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models.ViewModels
{
    public class ChampionVm
    {
        public int ChampionId { get; set; }
        [Required]
        public string ChampionTlAR { get; set; }
        [Required]
        public string ChampionTlEn { get; set; }

        [Required]

        public DateTime StartDate { get; set; }
        [Required]

        public DateTime EndDate { get; set; }
        [Required]

        public DateTime JoinStart { get; set; }
        [Required]

        public DateTime JoinEnd { get; set; }
        [Required]
        public IFormFile ChampionPic { get; set; }
        [Required]
        public string ChampionContent { get; set; }
        [Required]

        public int GameModeId { get; set; }

        [Required]

        public int ChampTypeId { get; set; }
    }
}
