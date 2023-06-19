using App.Entities.Dtos.NewsCommentDtos;

namespace App.Web.Mvc.ViewModels
{
	public class CommentAddAjaxViewModel
	{
        public NewsCommentAddDto NewsCommentAddDto { get; set; }
        public NewsCommentDto NewsCommentDto { get; set; }

    }
}
