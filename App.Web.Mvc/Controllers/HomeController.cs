using App.Service.Abstract;
using App.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace App.Web.Mvc.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly INewsService _newsService;

		public HomeController(ILogger<HomeController> logger, INewsService newsService)
		{
			_logger = logger;
			_newsService = newsService;
		}

		public async Task<IActionResult> Index()
		{
			var news = await _newsService.GetAllByNonDeletedAndActiveAsync();
			return View(news.Data);
		}
	

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}