using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameON.Models
{
    public class Invite
    {
        public int Id { get; set; }

        public GameInTerm GameInTerm { get; set; }

        public int GameInTermId { get; set; }

        public ApplicationUser UserSend { get; set; }
        public string UserSendId { get; set; }

        public ApplicationUser UserReceive { get; set; }
        [Display(Name = "Who are you gonna invite?")]
        public string UserReceiveId { get; set; }

        public bool Accepted { get; set; }

        public bool Rejected { get; set; }

        public bool Pending { get; set; }
    }
}
