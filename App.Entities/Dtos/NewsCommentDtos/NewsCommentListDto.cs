using App.Entities.Concrete;

namespace App.Entities.Dtos.NewsCommentDtos
{
    public class NewsCommentListDto
    {
        public IList<NewsComment> NewsComments { get; set; }
    }
}
