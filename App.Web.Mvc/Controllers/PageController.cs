using App.Entities.Concrete;
using App.Service.Abstract;
using App.Web.Mvc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Web.Mvc.Controllers
{
    public class PageController : Controller
    {
		private readonly IPageService _pageService;
		private readonly UserManager<User> _userManager;

        public PageController(IPageService ipageService, UserManager<User> userManager)
        {
            _pageService = ipageService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Detail()
		{
			var page = await _pageService.GetAllPageAsync();
			var userList = await _userManager.Users.ToListAsync();

			return View(new UserAndPagesViewModel
			{
				PageListDto=page.Data,
				User=userList
			});
		}
		public IActionResult Contact()
		{
			return View();
		}
	}
}
