using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MVC.Infrastructure;
using MVC.Models;
using MVC.Repositories;
using MVC.Services;
using Ninject;

namespace MVC.Identity
{
	public class AuthenticatedAttribute : AuthorizeAttribute
	{
		public UserRole[] AccessRole { get; set; }
        
	    public IAuthenticationService _authenticationService { get; set; }
	    public IUserService UserService { get; set; }

	    
	    public AuthenticatedAttribute()
	    {
	        var dbFactory = new DbFactory();
            _authenticationService = new FormsAuthenticationService(new UserService(new UserRepository(dbFactory), new UnitOfWork(dbFactory)));
            AccessRole = new UserRole[0];
        }

		public AuthenticatedAttribute(params UserRole[] accessRole) { AccessRole = accessRole; }

		protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
		{
            User user = _authenticationService.CurrentUser;
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