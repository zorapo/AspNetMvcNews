using App.Entities.Concrete;
using App.Service.Abstract;
using App.Shared.Utilities.Results.ComplexTypes;
using App.Web.Mvc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;


namespace App.Web.Mvc.Controllers
{
	public class NewsController : Controller
	{
		private readonly INewsService _newsService;
		private readonly ICategoryService _categoryService;
		private readonly INewsCommentService _newsCommentService;
		private readonly UserManager<User> _userManager;

		public NewsController(INewsService newsService, ICategoryService categoryService, INewsCommentService newsCommentService, UserManager<User> userManager)
		{
			_newsService = newsService;
			_categoryService = categoryService;
			_newsCommentService = newsCommentService;
			_userManager = userManager;
		}

		public async Task<IActionResult> Search(string keyword, int? pageNo)
		{
			int pageSize = 6;
			int pageIndex = 1;

			pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
			var search = await _newsService.SearchAsync(keyword, pageIndex, pageSize, false);
			if (search != null)
			{
				return View(new SearchViewModel
				{
					NewsList = search,
					Keyword = keyword,
				});
			}
			return NotFound();
		}
		//[Route("{title}/{id}")]
		public async Task<IActionResult> Detail(int id)
		{
			var news = await _newsService.GetAsync(id);
			var comments = await _newsCommentService.GetNewsCommentsAsync(id);

			return View(new NewsAndCommentsViewModel
			{
				NewsDto = news.Data,
				NewsCommentsList = comments.Data,
				
			});

		}
		[HttpPost]
		public async Task<IActionResult> Detail(NewsAndCommentsViewModel model, int id)
		{
			if (model.NewsComments.Email == null)
			{
				TempData["MessageError"] = "Unexpected error ! Please try again.";
				return RedirectToAction("Detail", id); ;
			}
			var user = await _userManager.FindByEmailAsync(model.NewsComments.Email);
			if (user != null)
			{

				model.NewsComments.UserId = user.Id;
				model.NewsComments.NewsId = id;
				var result = await _newsCommentService.AddAsync(model.NewsComments);
				if (result.ResultStatus == ResultStatus.Success)
				{
					TempData["MessageSuccess"] = "Your comment send successfully. Please wait for confirm your comment.";
					return RedirectToAction("Detail", id);
				}
				ModelState.AddModelError("", result.Message);
				TempData["MessageError"] = "Unexpected error ! Please try again.";
			}		
			TempData["MessageError"] = "Unexpected error ! Please try again.";
			return RedirectToAction("Detail", id); ;

		}

	}
}
