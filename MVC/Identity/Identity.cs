using System;
using System.Web;
using System.Web.Security;
using MVC.Infrastructure;
using MVC.Models;
using MVC.Services;

namespace MVC.Identity
{
	public interface IAuthenticationService
	{

		void Login(User user, bool rememberMe);

		void Logoff();

		User CurrentUser { get; }

	}

	public class FormsAuthenticationService : IAuthenticationService
	{
		private const string AUTH_COOKIE_NAME = "AuthCookie";
		//private const string SALT = "AuthCookie";

	    private IUserService _userService;

		public FormsAuthenticationService(IUserService userService)
		{
			_userService = userService;
		}

		public FormsAuthenticationService()
		{
		}

		public void Login(User user, bool rememberMe)
		{
			DateTime expiresDate = DateTime.Now.AddMinutes(310);
			if (rememberMe)
				expiresDate = expiresDate.AddDays(10);


			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
				1,
				user.UserID.ToString(),
				DateTime.Now,
				expiresDate, rememberMe, user.UserID.ToString());
			string encryptedTicket = FormsAuthentication.Encrypt(ticket);

			SetValue(AUTH_COOKIE_NAME, encryptedTicket, expiresDate);


			_currentUser = user;
		}

		public void Logoff()
		{
			SetValue(AUTH_COOKIE_NAME, null, DateTime.Now.AddYears(-1));
			_currentUser = null;
		}
		

		public static Boolean VerifyPassword(String password, Byte[] hash)
		{
			return Security.VerifyPassword(password,hash);
		}

		private User _currentUser;
		public User CurrentUser
		{
			get
			{
				if (_currentUser == null)
				{
					try
					{
						object cookie = HttpContext.Current.Request.Cookies[AUTH_COOKIE_NAME] != null ? HttpContext.Current.Request.Cookies[AUTH_COOKIE_NAME].Value : null;
						if (cookie != null && !string.IsNullOrEmpty(cookie.ToString()))
						{
							var ticket = FormsAuthentication.Decrypt(cookie.ToString());
							if (ticket != null)
							{
								var userId = Int32.Parse(ticket.UserData);
								_currentUser = _userService.GetUserByUserID(userId);
							}
						}

					}
					catch (Exception ex)
					{
						_currentUser = null;
					}
				}
				return _currentUser;
			}
		}

		public static void SetValue(string cookieName, string cookieObject, DateTime dateStoreTo)
		{

			HttpCookie cookie = HttpContext.Current.Response.Cookies[cookieName] ?? new HttpCookie(cookieName) {Path = "/"};

			cookie.Value = cookieObject;
			cookie.Expires = dateStoreTo;

			HttpContext.Current.Response.SetCookie(cookie);
		}
	}
}