using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Identity;
using MVC.Infrastructure;
using MVC.Models;
using MVC.ViewModels;
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
                        _authenticationService.Login(user, userView.IsPersistent);
                        if (Request.IsAjaxRequest()) return View("LoginForm", userView);
                        return RedirectToAction("List", "Characters");
                    }
                }

            }
            if (Request.IsAjaxRequest()) return View("LoginForm", userView);
            return View(userView);
        }

        public ActionResult Logout()
        {
            _authenticationService.Logoff();
            if (Request.IsAjaxRequest()) return View("LoginForm", null);
            return RedirectToAction("List", "Characters");
        }

        [ChildActionOnly]
        public ActionResult LoginForm()
        {
            var user = _authenticationService.CurrentUser;
            UserViewModel userViewModel = user == null?null : new UserViewModel(user); 
            
            return View(userViewModel);
        }

        public ActionResult Register()
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
        public ActionResult Register(UserViewModel userView)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    LoginName = userView.Login,
                    PwdHash = Security.GenerateHash(userView.Password),
                    Role = UserRole.User,
                    Votes = new List<Vote>()
                };
                if (userService.GetUserByLoginName(userView.Login) != null)
                {
                    ModelState.AddModelError("Login", "A user with this name alreay exists");
                    return View();
                }
                userService.CreateUser(user);
                userService.Commit();

                _authenticationService.Login(user, userView.IsPersistent);
            }

            return RedirectToAction("List", "Characters");
        }
    }
}