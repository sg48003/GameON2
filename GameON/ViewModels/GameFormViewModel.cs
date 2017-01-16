using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameON.Models;

namespace GameON.ViewModels
{
    public class GameFormViewModel
    {
        public IEnumerable<GameType> GameTypes { get; set; }
        public Game Game { get; set; }
    }
}