using App.Entities.Concrete;
using App.Entities.Dtos;
using App.Service.Abstract;
using App.Shared.Utilities.Results.ComplexTypes;
using App.Web.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsService _newsService;
        private readonly INewsCommentService _newsCommentService;
        private readonly UserManager<User> _userManager;

        public HomeController(INewsCommentService newsCommentService, INewsService newsService, ICategoryService categoryService, UserManager<User> userManager)
        {
            _newsCommentService = newsCommentService;
            _newsService = newsService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var categoriesCount = await _categoryService.CountByNonDeletedAsync();
            var newsCount = await _newsService.CountByNonDeletedAsync();
            var newsCommentsCount = await _newsCommentService.CountByNonDeletedAsync();
            var userCount = await _userManager.Users.CountAsync();
            var newsList = await _newsService.GetAllAsync();

            if (categoriesCount.ResultStatus == ResultStatus.Success && newsCount.ResultStatus == ResultStatus.Success && newsCommentsCount.ResultStatus == ResultStatus.Success && userCount > -1 && newsList.ResultStatus == ResultStatus.Success)
            {
                return View(new AdminDashboardDto
                {
                    CategoriesCount = categoriesCount.Data,
                    NewsCount = newsCount.Data,
                    NewsCommentsCount = newsCommentsCount.Data,
                    UsersCount = userCount,
                    NewsList = newsList.Data
                });
            }
            return NotFound();

        }
        public async Task<IActionResult> UserIndex()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            return View(new UserAndRolesViewModel
            {
                User = user,
                Roles = roles
            });      
        }
    }
}
