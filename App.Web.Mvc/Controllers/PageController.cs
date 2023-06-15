using App.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Controllers
{
    public class PageController : Controller
    {
		private readonly IPageService _pageService;

		public PageController(IPageService ipageService)
		{
			_pageService = ipageService;
		}
		public async Task<IActionResult> Detail()
		{
			var page = await _pageService.GetAllPageAsync();

			return View(page);
		}
		public IActionResult Contact()
		{
			return View();
		}
	}
}
