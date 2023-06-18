using App.Entities.Dtos.NewsCommentDtos;
using App.Service.Abstract;
using App.Shared.Utilities.Results.ComplexTypes;
using App.Shared.Utilities.Results.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Controllers
{
	public class CommentController : Controller
	{
		private readonly INewsCommentService _newsCommentService;

		public CommentController(INewsCommentService newsCommentService)
		{
			_newsCommentService = newsCommentService;
		}

        [Route("CommentAdd"), HttpPost]
        public async Task<IActionResult> Add(NewsCommentAddDto newsCommentAddDto)
		{
			if (ModelState.IsValid)
			{
				var result = await _newsCommentService.AddAsync(newsCommentAddDto);
				if (result.ResultStatus == ResultStatus.Success)
				{
					return RedirectToAction("Detail","News",newsCommentAddDto.NewsId);
				}
				ModelState.AddModelError("", result.Message);
			}
			ModelState.AddModelError("", "Bir hata oluştu.Yorumunuz gönderilemedi.");
			return RedirectToAction("Detail", "News", newsCommentAddDto.NewsId);
		}
	}
}
