using App.Entities.Dtos;
using App.Service.Abstract;
using App.Web.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Controllers
{
	public class NewsController : Controller
    {
		private readonly INewsService _newsService;

		public NewsController(INewsService newsService)
		{
			_newsService = newsService;
		}

		public async Task<IActionResult> Search(string keyword,int pageNo=1,int pageSize=6)
        {
            var search=await _newsService.SearchAsync(keyword,pageNo,pageSize,false);
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
        public IActionResult Detail(int id)
        {
            var news = _newsService.GetAsync(id);
            return View(news.Result.Data);
        }
    }
}
