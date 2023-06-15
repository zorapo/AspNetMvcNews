using App.Entities.Dtos;
using App.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Controllers
{
	public class CategoryController : Controller
    {

        private readonly ICategoryService _categoryService;
        private readonly INewsService _newsService;

        public CategoryController(INewsService newsService, ICategoryService categoryService)
        {

            _newsService = newsService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int categoryId,int? pageNo)
        {
			int pageSize = 6;
			int pageIndex = 1;
		
			pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
			var news = await _newsService.GetNewsByCategoryIdAsync(categoryId);
            var category= await _categoryService.GetAsync(categoryId);
            var categoriesList= await _categoryService.GetAllAsync();
            var pagedNews= await _newsService.GetAllNewsByCategoryPagingAsync(categoryId, pageIndex, pageSize,false);
			return View(new CategoryPageDto
			{
                NewsList=news.Data,
                Category=category.Data.Category,
                PagedNews=pagedNews,
                Categories=categoriesList.Data
            });
        }
    }
}
