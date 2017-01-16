using GameON.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameON.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        //public string Image { get; set; }

        public GameTypeDto GameType { get; set; }

        [Display(Name = "Game Type")]
        public int GameTypeId { get; set; }

        //min i max broj osoba prebaceno u termin!!!
    }
}