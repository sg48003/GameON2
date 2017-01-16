using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameON.Dtos
{
    public class PlayerDto
    {
        public int Id { get; set; }

        public GameInTermDto GameInTerm { get; set; }
        public int GameInTermId { get; set; }

        public UserDto User { get; set; }
        public string UserId { get; set; }
    }
}