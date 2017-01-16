using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameON.Models;

namespace GameON.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private ApplicationDbContext _context;

        public MembersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Members
        public ActionResult Index()
        {
            var members = _context.Users.ToList();

            return View(members);
        }
    }
}