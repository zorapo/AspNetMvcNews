using App.Entities.Concrete;
using App.Web.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Areas.Admin.ViewComponents
{
	public class AdminMenuViewComponent : ViewComponent
	{
		private readonly UserManager<User> _userManager;

		public AdminMenuViewComponent(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public IViewComponentResult Invoke()
		{
			var user = _userManager.GetUserAsync(HttpContext.User).Result;
			var roles =  _userManager.GetRolesAsync(user).Result;
			return View(new UserAndRolesViewModel
			{
				User = user,
				Roles = roles
			});
		}
	}
}
