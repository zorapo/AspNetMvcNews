using App.Entities.Dtos.NewsCommentDtos;
using App.Service.Abstract;
using App.Web.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;

        public NewsController(INewsService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
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
           //news.Data.News.CategoryId
            return View(new NewsAndCommentsViewModel
            {
                NewsDto=news.Data,
                NewsComments=new NewsCommentAddDto
                {
                    NewsId=news.Data.News.Id
                }
            });
        }
      
	}
}
