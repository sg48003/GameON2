using DayPilot.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DayPilot.Web.Mvc.Events.Month;

namespace GameON.Models
{
    public class Dpm : DayPilotMonth
    {
        protected override void OnInit(InitArgs e)
        {
            var _context = new ApplicationDbContext();
            var events = from gt in _context.GameInTerms
                         join t in _context.Terms on gt.TermId equals t.Id
                         select new
                         {
                             text = gt.Game.Name,
                             id = t.Id,
                             start = t.StartTime,
                             end = t.EndTime
                         };
            DataIdField = "id";
            DataTextField = "text";
            DataStartField = "start";
            DataEndField = "end";

                         

            Update();
        }
    }
}