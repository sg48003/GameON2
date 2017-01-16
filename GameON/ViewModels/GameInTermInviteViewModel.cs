using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameON.Models;

namespace GameON.ViewModels
{
    public class GameInTermInviteViewModel
    {
        public Invite Invite { get; set; }

        public GameInTerm gameInTerm { get; set; }

        public string UserSends { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}