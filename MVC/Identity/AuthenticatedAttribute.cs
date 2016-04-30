using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MVC.Models;

namespace MVC.Identity
{
	public class AuthenticatedAttribute : AuthorizeAttribute
	{
		public UserRole[] AccessRole { get; set; }
		public AuthenticatedAttribute()
		{
			AccessRole = new UserRole[0];
		}

		public AuthenticatedAttribute(params UserRole[] accessRole) { AccessRole = accessRole; }

		protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
		{
			FormsAuthenticationService frAuthenticationService = new FormsAuthenticationService();
			
			User user = frAuthenticationService.CurrentUser;
			if (user == null)
				return false;

			if (AccessRole.Length == 0)
				return true;

			return AccessRole.Contains(user.Role);
		}

		protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
		{
			filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {controller = "Account",action = "Login"}));
		}

	}
}