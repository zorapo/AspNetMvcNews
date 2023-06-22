using App.Entities.Concrete;
using App.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
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
        public async Task<IActionResult> Details(int contactId)
        {
            var model = await _contactService.GetContactAsync(contactId);
            return View(model.Data);
        }
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
