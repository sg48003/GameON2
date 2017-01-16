using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameON.Models;

namespace GameON.ViewModels
{
    public class GameInTermIndexViewModel
    {
        public IEnumerable<GameInTerm> GameInTerm { get; set; }
        
        public string CurrentUser { get; set; }
        public IEnumerable<Player> Players { get; set; }
    }
}