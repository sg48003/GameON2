using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameON.Models
{
    public class GameInTerm
    {
        public int Id { get; set; }

        public Game Game { get; set; }
        [Display(Name = "Game")]
        public int GameId { get; set; }

        public Term Term { get; set; }
        public int TermId { get; set; }

        public bool Confirmed { get; set; }

        public bool Canceled { get; set; }

        public bool Pending { get; set; }
    }
}