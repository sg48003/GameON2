using GameON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GameON.Controllers.API
{
    public class EventsController : ApiController
    {
        private ApplicationDbContext _context;
        public EventsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET api/Event
        public IQueryable GetEvents()

        {
            var events = from e in _context.Terms
                         join g in _context.GameInTerms on e.Id equals g.TermId
                         select new {
                             id = e.Id,
                             start = e.StartTime,
                             end = e.EndTime,
                             title = g.Game.Name
                         };


            return events;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
