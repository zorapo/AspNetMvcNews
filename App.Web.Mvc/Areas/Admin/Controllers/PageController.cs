using App.Entities.Concrete;
using App.Service.Abstract;
using App.Web.Mvc.Helpers.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Editor")]
	public class PageController : Controller
	{
		private readonly IPageService _pageService;
		private readonly IImageHelper _imageHelper;

		public PageController(IPageService pageService, IImageHelper imageHelper)
		{
			_pageService = pageService;
			_imageHelper = imageHelper;
		}

		public async Task<IActionResult> Index()
		{
			var model = await _pageService.GetAllPageAsync();
			return View(model);
		}
		public IActionResult Details(int id)
		{
			var model = _pageService.GetPageAsync(id);
			return View(model);
		}
		public IActionResult Create()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Page page)
		{

			if (page == null || !ModelState.IsValid)
			{
				return RedirectToAction(nameof(Index));
			}
			await _pageService.AddAsync(page);
			return RedirectToAction(nameof(Index));

		}
		public async Task<IActionResult> Edit(int id)
		{
			var model = await _pageService.GetPageAsync(id);
			return View(model.Data);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(Page page)
		{
			if (page == null || !ModelState.IsValid)
				return RedirectToAction(nameof(Index));
			await _pageService.UpdateAsync(page);
			return RedirectToAction(nameof(Index));

		}
		public async Task<IActionResult> Delete(int id)
		{
			var model = await _pageService.GetPageAsync(id);
			await _pageService.DeleteAsync(model.Data);
			return RedirectToAction(nameof(Index));

		}
	}
}
