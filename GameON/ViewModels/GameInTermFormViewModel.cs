using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameON.Models;

namespace GameON.ViewModels
{
    public class GameInTermFormViewModel
    {
        public IEnumerable<Game> Games { get; set; }
        public Term Term { get; set; }
        public GameInTerm GameInTerm { get; set; }
    }
}