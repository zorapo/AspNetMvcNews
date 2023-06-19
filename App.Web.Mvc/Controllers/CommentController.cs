using App.Entities.Concrete;
using App.Entities.Dtos.NewsCommentDtos;
using App.Service.Abstract;
using App.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Web.Mvc.Controllers
{
	public class CommentController : Controller
	{
		private readonly INewsCommentService _newsCommentService;
		private readonly UserManager<User> _userManager;

		public CommentController(INewsCommentService newsCommentService, UserManager<User> userManager)
		{
			_newsCommentService = newsCommentService;
			_userManager = userManager;
		}

		[HttpPost]
		public async Task<JsonResult> Add(NewsCommentAddDto newsCommentAddDto)
		{

			var user = await _userManager.FindByEmailAsync(newsCommentAddDto.Email);		
			newsCommentAddDto.UserId = user.Id;
			var result = await _newsCommentService.AddAsync(newsCommentAddDto);
			if (result.ResultStatus == ResultStatus.Success)
			{
				var addedComment = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
				{
					ReferenceHandler = ReferenceHandler.Preserve
				});
				return Json(addedComment);
			
			}			
			ModelState.AddModelError("", result.Message);
			var CommentError = JsonSerializer.Serialize(newsCommentAddDto, new JsonSerializerOptions
			{
				ReferenceHandler = ReferenceHandler.Preserve
			});
			return Json(CommentError);
		}
	}
}
