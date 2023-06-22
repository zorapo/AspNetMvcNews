using App.Entities.Concrete;
using App.Service.Abstract;
using App.Web.Mvc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Web.Mvc.Controllers
{
    public class PageController : Controller
    {
		private readonly IPageService _pageService;
        private readonly IContactService _contactService;
		private readonly UserManager<User> _userManager;

        public PageController(IPageService ipageService, UserManager<User> userManager, IContactService contactService)
        {
            _pageService = ipageService;
            _userManager = userManager;
            _contactService = contactService;
        }
        public async Task<IActionResult> Detail()
		{
			var page = await _pageService.GetAllPageAsync();
			var userList = await _userManager.GetUsersInRoleAsync("Admin");

			return View(new UserAndPagesViewModel
			{
				PageListDto=page.Data,
				User=userList
			});
		}
		public IActionResult Contact()
		{
			return View();
		}
        [HttpPost]
        public async Task<IActionResult> Contact(Contact page)
        {
            if (page == null || !ModelState.IsValid)
            {
				ModelState.AddModelError("", "Hata Oluştu! Mesajınız Gönderilemedi!");
				return RedirectToAction(nameof(Index));
            }

            await _contactService.AddContactAsync(page);
			TempData["Mesaj"] = "<div class = 'alert alert-success'>Mesajınız Gönderildi. Teşekkürler...</div>";
			return View();

        }
    }
}
