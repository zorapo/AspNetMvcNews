using App.Entities.Dtos.NewsCommentDtos;
using App.Entities.Dtos.NewsDtos;

namespace App.Web.Mvc.ViewModels
{
	public class NewsAndCommentsViewModel
	{
        public NewsAndCommentsViewModel()
        {
            NewsComments=new NewsCommentAddDto();
        }
        public NewsDto NewsDto { get; set; }
        public NewsCommentAddDto NewsComments { get; set; }
        public NewsCommentListDto NewsCommentsList { get; set; }

    }
}
