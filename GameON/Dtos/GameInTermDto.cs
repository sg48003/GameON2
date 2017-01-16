using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameON.Dtos
{
    public class GameInTermDto
    {
        public int Id { get; set; }

        public GameDto Game { get; set; }
      
        public int GameId { get; set; }

        public TermDto Term { get; set; }
        public int TermId { get; set; }
    }
}