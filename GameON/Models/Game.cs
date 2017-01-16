using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameON.Models
{

    public class Game
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]

        [Display(Name = "Game")]
        public string Name { get; set; }

        public GameType GameType { get; set; }

        [Display(Name = "Game Type")]
        public int GameTypeId { get; set; }
    }
}
