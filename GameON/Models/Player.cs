using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameON.Models
{
    public class Player
    {
        public int Id { get; set; }

        public GameInTerm GameInTerm { get; set; }
        public int GameInTermId { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}