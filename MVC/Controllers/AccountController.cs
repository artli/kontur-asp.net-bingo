using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Identity;
using MVC.Infrastructure;
using MVC.Models;
using MVC.Services;

namespace MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly ICharacterService characterService;
        private readonly ICartProvider cartProvider;
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IUserService userService, ICharacterService characterService, ICartProvider cartProvider, IAuthenticationService authenticationService)
        {
            this.userService = userService;
            this.characterService = characterService;
            this.cartProvider = cartProvider;
            this._authenticationService = authenticationService;
        }
        // GET: Account
        public ActionResult Login()
        {
            if (_authenticationService.CurrentUser != null)
            {
                var user = userService.GetUserByUserID(_authenticationService.CurrentUser.UserID);
                if ((user != null))
                {
                    return RedirectToAction("List", "Characters");
                }

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserViewModel userView)
        {
            if (ModelState.IsValid)
            {
                var user = userService.GetUserByLoginName(userView.Login);
                if (user != null)
                {
                    if (Security.VerifyPassword(userView.Password, user.PwdHash))
                    {
                        _authenticationService.Login(user,userView.IsPersistent);
                        return RedirectToAction("List", "Characters");
                    }
                }

            }
            return View(userView);
        }

        public ActionResult Logout()
        {
            _authenticationService.Logoff();
            return RedirectToAction("List", "Characters");
        }
        public ActionResult TestUser(string username, string pwd)
        {
            var user = new User()
            {
                LoginName = username,
                PwdHash = Security.GenerateHash(pwd),
                Role = UserRole.User,
                Votes = new List<Vote>()
            };
            userService.CreateUser(user);
            userService.Commit();
            return RedirectToAction("Login");

        }
    }
}