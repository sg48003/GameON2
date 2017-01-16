using GameON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using GameON.ViewModels;
using Microsoft.AspNet.Identity;

namespace GameON.Controllers
{
    public class GamesController : Controller
    {
        private ApplicationDbContext _context;

        public GamesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Games
        public ActionResult Index()
        {
            //var games = _context.Games.Include(k => k.GameType).ToList();
            if (User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                return View("ReadOnlyIndex");
            }
            
        }

        [Authorize(Roles ="Admin")]
        public ActionResult New()
        {
            var gameTypes = _context.GameTypes.ToList();
            var viewModel = new GameFormViewModel()
            {
                GameTypes = gameTypes

            };
            return View("GameForm",viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();

            return RedirectToAction("Index", "Games");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var gameInDb = _context.Games.SingleOrDefault(c => c.Id == id);

            if (gameInDb == null)
                return HttpNotFound();

            _context.Games.Remove(gameInDb);
            _context.SaveChanges();

            return RedirectToAction("Index", "Games");

        }

        public ActionResult Edit(int id)
        {
            var game = _context.Games.SingleOrDefault(c => c.Id == id);
            var gameTypes = _context.GameTypes.ToList();

            if (game == null)
                return HttpNotFound();

            var viewModel = new GameFormViewModel()
            {
                Game = game,
                GameTypes = gameTypes


            };
            return View("GameForm", viewModel);
        }

        public ActionResult Events(int id)
        {
            var game = _context.Games.SingleOrDefault(c => c.Id == id);
            if (game == null)
                return HttpNotFound();

            var events = _context.GameInTerms.Include(c => c.Term).Include(c =>c.Term.Owner).Where(c => c.GameId == id).ToList();

            var players = _context.Players.ToList();

            var userId = User.Identity.GetUserId();

            var viewModel = new GameInTermIndexViewModel()
            {
                GameInTerm = events,
                Players = players,
                CurrentUser = userId
            };
            return View(viewModel);
        }
    }
} 