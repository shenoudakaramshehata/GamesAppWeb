using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models
{
    public class ChampionSlider
    {
        [Key]
        public int SliderId { get; set; }
        public string pic { get; set; }
        public bool IsActivity { get; set; }
        public int SliderTypeId { get; set; }
        [ForeignKey("SliderTypeId")]
        public SliderType SliderType { get; set; }
        public int EntityId { get; set; }
        public int OrderIndex { get; set; }
    }
}
