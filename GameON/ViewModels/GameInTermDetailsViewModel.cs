using GameON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameON.ViewModels
{
    public class GameInTermDetailsViewModel
    {
        public GameInTerm GameInTerm { get; set; }

        public string CurrentUser { get; set; }
        public IEnumerable<Player> Players { get; set; }
    }

}