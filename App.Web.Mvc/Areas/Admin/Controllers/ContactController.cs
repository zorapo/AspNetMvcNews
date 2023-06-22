using App.Entities.Concrete;
using App.Service.Abstract;
using App.Web.Mvc.Helpers.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactController : Controller
    {
        
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _contactService.GetAllContactAsync();
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var model = _contactService.GetContactAsync(id);
            return View(model);
        }
        //public IActionResult Create()
        //{

        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(Page page)
        //{

        //    if (page == null || !ModelState.IsValid)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    page.ImagePath = await _imageHelper.ImageUpload("pages", page.ImageFile, "page");
        //    await _pageService.AddAsync(page);
        //    return RedirectToAction(nameof(Index));

        //}
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _contactService.GetContactAsync(id);
            return View(model.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Contact page)
        {
            if (page == null || !ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            await _contactService.UpdateContactAsync(page);
            return RedirectToAction(nameof(Index));

        }
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var model = await _pageService.GetPageAsync(id);
        //    await _pageService.DeleteAsync(model.Data);
        //    return RedirectToAction(nameof(Index));

        //}
    }
}
