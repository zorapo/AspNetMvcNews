using App.Data.Concrete.EntityFramework.Context;
using App.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.ViewComponents
{
    public class NavbarViewComponent:ViewComponent
	{
		private readonly ICategoryService _categoryService;

        public NavbarViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
     
        }

        public IViewComponentResult Invoke()
		{
            var categories = _categoryService.GetAllByNonDeletedAndActiveAsync();
			return View("Navbar",categories.Result.Data); 
		}
	}
}
