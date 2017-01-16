using GameON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using GameON.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace GameON.Controllers
{
    public class GamesInTermsController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public GamesInTermsController()
        {
            _context = new ApplicationDbContext();
        }
        public GamesInTermsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: GamesInTerms
        public ActionResult Index()
        {
            var games = _context.GameInTerms.Include(k => k.Game).Include(k => k.Game.GameType).ToList();
            var terms = _context.GameInTerms.Include(k => k.Term).Include(k => k.Term.Owner).ToList();
            var gamesinterms = games.Union(terms);

            var players = _context.Players.ToList();

            var userId = User.Identity.GetUserId();

            var viewModel = new GameInTermIndexViewModel()
            {
                GameInTerm = gamesinterms,
                Players = players,
                CurrentUser = userId
            };
            return View(viewModel);
        }

        public ActionResult ShowPlayers(int id)
        {
            var players = _context.Players.Include(c => c.User).Where(c => c.GameInTermId == id).ToList();
          
            if (players == null)
                return HttpNotFound();

            return View("Players",players);
      
        }

        public ActionResult NewTerm()
        {
            return View("TermForm");
        }

        public ActionResult New(Term term)
        {
            var games = _context.Games.ToList();
            var terms = _context.Terms.SingleOrDefault(k => k.Id == term.Id);
            var viewModel = new GameInTermFormViewModel()
            {
                Games = games,
                Term = terms

            };
            return View("GameForm2", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(GameInTerm gameInTerm)
        {
            var players = _context.Players.Include(k => k.User).Where(k => k.GameInTermId == gameInTerm.Id).ToList();

            if (!ModelState.IsValid)
            {
                var viewModel = new GameInTermFormViewModel()
                {
                    GameInTerm = gameInTerm,
                    Term = gameInTerm.Term,
                    Games = _context.Games.ToList()
                };
                return View("GameInTermForm", viewModel);
            }


            if (gameInTerm.Id == 0)
            {
                gameInTerm.Pending = true;
                _context.GameInTerms.Add(gameInTerm);
            }
            else
            {
                var gameInTermInDb = _context.GameInTerms.Include(k => k.Game).Include(c => c.Term).Single(c => c.Id == gameInTerm.Id);

                gameInTermInDb.Term.StartTime = gameInTerm.Term.StartTime;
                gameInTermInDb.Term.EndTime = gameInTerm.Term.EndTime;
                gameInTermInDb.Term.Deadline = gameInTerm.Term.Deadline;
                gameInTermInDb.Term.Taken = gameInTerm.Term.Taken;
                gameInTermInDb.Term.Price = gameInTerm.Term.Price;
                gameInTermInDb.Term.City = gameInTerm.Term.City;
                gameInTermInDb.Term.Address = gameInTerm.Term.Address;
                gameInTermInDb.Term.Place = gameInTerm.Term.Place;
                gameInTermInDb.Term.ZIPCode = gameInTerm.Term.ZIPCode;
                gameInTermInDb.Term.MinPeople = gameInTerm.Term.MinPeople;
                gameInTermInDb.Term.MaxPeople = gameInTerm.Term.MaxPeople;
                gameInTermInDb.GameId = gameInTerm.GameId;
          

                var message = new IdentityMessage();
                foreach (var player in players)
                {
                   
                    message.Destination = player.User.PhoneNumber;
                    message.Body = "The event: " + gameInTermInDb.Game.Name + " at " + gameInTermInDb.Term.StartTime.ToString("dd MMM yyyy HH:mm") + " was edited";
                    await UserManager.SmsService.SendAsync(message);
                }


            }


            _context.SaveChanges();

            return RedirectToAction("Index", "GamesInTerms");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTerm(Term term)
        {
            if (!ModelState.IsValid)
            {
       
                return View("TermForm", term);
            }

            term.OwnerId = User.Identity.GetUserId();
            _context.Terms.Add(term);
            _context.SaveChanges();

            return RedirectToAction("New", "GamesInTerms", term);
        }

        public ActionResult Delete(int id)
        {
            var gamesInTermsInDb = _context.GameInTerms.SingleOrDefault(c => c.Id == id);

            if (gamesInTermsInDb == null)
                return HttpNotFound();

            _context.GameInTerms.Remove(gamesInTermsInDb);
            _context.SaveChanges();

            return RedirectToAction("Index", "GamesInTerms");

        }
        
        public ActionResult JoinLeave(int id)
        {
            var userId = User.Identity.GetUserId();
            var joinedPlayer = _context.Players.Where(c => c.UserId == userId).Where(c => c.GameInTermId == id).ToList().FirstOrDefault();

            var players = _context.Players.Where(c => c.GameInTermId == id).Count();
            var gameInTerm = _context.GameInTerms.Include(c => c.Term).SingleOrDefault(c => c.Id == id);
           
            if (joinedPlayer != null)
            {
                _context.Players.Remove(joinedPlayer);
                players--;
            }
            else
            {
                var player = new Player();
                player.GameInTermId = id;
                player.UserId = userId;
                _context.Players.Add(player);
                players++;
            }

            if (players == gameInTerm.Term.MaxPeople)
            {
                gameInTerm.Term.Taken = true;
            
            }
            else
            {
                gameInTerm.Term.Taken = false;
            }

            TryUpdateModel(gameInTerm);
            _context.SaveChanges();


            return RedirectToAction("Index", "GamesInTerms");
        }

        public ActionResult Details(int id)
        {
            var gamesinterms = _context.GameInTerms.Include(k => k.Game).Include(k => k.Game.GameType).Include(k => k.Term).Include(k => k.Term.Owner).SingleOrDefault(c => c.Id == id);
            var players = _context.Players.Include(k => k.User).Where(k => k.GameInTermId == id).ToList();
            if (gamesinterms == null)
                return HttpNotFound();

            var viewModel = new GameInTermDetailsViewModel()
            {
                GameInTerm = gamesinterms,
                Players = players,
                CurrentUser = User.Identity.GetUserId()
            };

            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var gameInTerm = _context.GameInTerms.Include(c => c.Term).SingleOrDefault(c => c.Id == id);
            var games = _context.Games.ToList();

            if (gameInTerm == null)
                return HttpNotFound();

            var viewModel = new GameInTermFormViewModel()
            {
                GameInTerm = gameInTerm,
                Term = gameInTerm.Term,
                Games = games
               

            };
            return View("GameInTermForm",viewModel);


        }

        public ActionResult Invite(int id)
        {
            var gameInTerm = _context.GameInTerms.Include(c => c.Term).SingleOrDefault(c => c.Id == id);
            var user = User.Identity.GetUserId();
            var users = _context.Users.Where(m => m.Id != user).ToList();

            if (gameInTerm == null)
                return HttpNotFound();

            var viewModel = new GameInTermInviteViewModel()
            {
                gameInTerm = gameInTerm,
                Users = users,
                UserSends = User.Identity.GetUserId()
            }; 

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInvite(Invite invite)
        {
            if (!ModelState.IsValid)
            {

                return View("Invite", invite);
            }

            invite.Pending = true;
            invite.UserSendId = User.Identity.GetUserId();
           

            _context.Invites.Add(invite);
            _context.SaveChanges();

            return RedirectToAction("Index", "GamesInTerms");
        }

        public ActionResult AvailableEvents()
        {
            var games = _context.GameInTerms.Include(k => k.Game).Include(k => k.Game.GameType).ToList();
            var terms = _context.GameInTerms.Include(k => k.Term).Include(k => k.Term.Owner).ToList();
            var gamesinterms = games.Union(terms).Where(k => k.Term.Taken == false);

            var players = _context.Players.ToList();

            var userId = User.Identity.GetUserId();

            var viewModel = new GameInTermIndexViewModel()
            {
                GameInTerm = gamesinterms,
                Players = players,
                CurrentUser = userId
            };
            return View("AvailableEvents",viewModel);
        }
        
        //public ActionResult Backend()
        //{
        //    return new Dpm().CallBack(this);
        //}
        

    }
}