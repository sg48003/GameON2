using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GameON.Models;
using System.Data.Entity;

namespace GameON.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;

        public ManageController()
        {
            _context = new ApplicationDbContext();
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : message == ManageMessageId.Confirmed ? "The event has been confirmed."
                : message == ManageMessageId.Canceled ? "The event has been canceled."
                : message == ManageMessageId.EventOutdated ? "The event is outdated. It was automaticaly canceled."
                : message == ManageMessageId.InviteJoin ? "You have joined the event."
                : message == ManageMessageId.InviteOutdated ? "The event is outdated. Your invite was automaticaly deleted."
                : message == ManageMessageId.InviteDecline ? "You have declined the invite."
                : message == ManageMessageId.InviteFullorTaken ? "The event was taken. Your invite was automaticaly deleted."
                : "";


            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                gamesInTermsCreated = _context.GameInTerms.Include(k => k.Term).Include(k => k.Game).Where(k => k.Term.OwnerId == userId).ToList(),
                gamesInTermsJoined = _context.Players.Include(k => k.GameInTerm).Include(k => k.GameInTerm.Term).Include(k => k.GameInTerm.Game).Where(k => k.UserId == userId).ToList(),
                players = _context.Players.ToList(),
                Invites = _context.Invites.Include(k => k.UserSend).Where(k => k.UserReceiveId == userId),
                CurrentUser = userId
                
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });

        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
            
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            _context.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Accept(int id)
        {

            var invite = _context.Invites.SingleOrDefault(k => k.Id == id);
            var gameInTerm = _context.GameInTerms.Include(k => k.Term).SingleOrDefault(k => k.Id == invite.GameInTermId);


            if (invite == null)
                return HttpNotFound();
            if (invite.GameInTerm.Term.Deadline < DateTime.Now)
            {
                _context.Invites.Remove(invite);
                _context.SaveChanges();
                return RedirectToAction("Index", "Manage", new { Message = ManageMessageId.InviteOutdated });
            }

            
            if (gameInTerm.Term.Taken == true)
            {
                _context.Invites.Remove(invite);
                _context.SaveChanges();
                return RedirectToAction("Index", "Manage", new { Message = ManageMessageId.InviteFullorTaken });
            }

            invite.Pending = false;
            invite.Accepted = true;

            Player player = new Player()
            {
                GameInTermId = invite.GameInTermId,
                UserId = invite.UserReceiveId,
                User = invite.UserReceive
            };

            _context.Players.Add(player);
            _context.SaveChanges();

            var players = _context.Players.Where(k => k.GameInTermId == invite.GameInTermId).ToList();

            if (players.Count() >= gameInTerm.Term.MaxPeople)
            {
                gameInTerm.Term.Taken = true;
                _context.SaveChanges();
            }


            
            return RedirectToAction("Index", "Manage", new { Message = ManageMessageId.InviteJoin });
        }
        public ActionResult Reject(int id)
        {
            var invite = _context.Invites.SingleOrDefault(k => k.Id == id);

            if (invite == null)
                return HttpNotFound();

            if (invite.GameInTerm.Term.Deadline < DateTime.Now)
            {
                _context.Invites.Remove(invite);
                _context.SaveChanges();
                return RedirectToAction("Index", "Manage", new { Message = ManageMessageId.InviteOutdated });
            }

            invite.Pending = false;
            invite.Rejected = true;
            _context.SaveChanges();

            return RedirectToAction("Index", "Manage", new { Message = ManageMessageId.InviteDecline });
        }

        public async Task<ActionResult> Confirm(int id)
        {
            var gameInTerm = _context.GameInTerms.Include(k => k.Game).Include(k => k.Term).SingleOrDefault(k => k.Id == id);
            var players = _context.Players.Include(k => k.User).Where(k => k.GameInTermId == id).ToList();

            if (gameInTerm == null)
                return HttpNotFound();

            if (gameInTerm.Term.Deadline < DateTime.Now)
            {
                gameInTerm.Canceled = true;
                gameInTerm.Pending = false;
                _context.SaveChanges();
                return RedirectToAction("Index", "Manage", new { Message = ManageMessageId.EventOutdated });
            }


            //salji podatke ostalima preko mobitela da je confirmed i da su podaci o igracima na aplikaciji
            var message = new IdentityMessage();
            foreach (var player in players)
            {

                message.Destination = player.User.PhoneNumber;
                message.Body = "The event: " + gameInTerm.Game.Name + " at " + gameInTerm.Term.StartTime.ToString("dd MMM yyyy HH:mm") + " was confirmed. Other player's info may be found in details when you click the event in your \"Events you joined\" tab";
                await UserManager.SmsService.SendAsync(message);
            }

            gameInTerm.Confirmed = true;
            gameInTerm.Pending = false;
            _context.SaveChanges();

            return RedirectToAction("Index", "Manage", new { Message = ManageMessageId.Confirmed });
        }

        public async Task<ActionResult> Cancel(int id)
        {
            var gameInTerm = _context.GameInTerms.Include(k => k.Game).Include(k => k.Term).SingleOrDefault(k => k.Id == id);
            var players = _context.Players.Include(k => k.User).Where(k => k.GameInTermId == id).ToList();

            if (gameInTerm == null)
                return HttpNotFound();

            gameInTerm.Canceled = true;
            gameInTerm.Pending = false;
            _context.SaveChanges();

            if (gameInTerm.Term.Deadline < DateTime.Now)
            {
               
                return RedirectToAction("Index", "Manage", new { Message = ManageMessageId.EventOutdated });
            }

            var message = new IdentityMessage();
            foreach (var player in players)
            {

                message.Destination = player.User.PhoneNumber;
                message.Body = "The event: " + gameInTerm.Game.Name + " at " + gameInTerm.Term.StartTime.ToString("dd MMM yyyy HH:mm") + " was canceled.";
                await UserManager.SmsService.SendAsync(message);
            }

            return RedirectToAction("Index", "Manage", new { Message = ManageMessageId.Canceled });
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error,
            Confirmed,
            Canceled,
            InviteJoin,
            InviteDecline,
            InviteOutdated,
            EventOutdated,
            InviteFullorTaken
               
        }


#endregion
    }
}