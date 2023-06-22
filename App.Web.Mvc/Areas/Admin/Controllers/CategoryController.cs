using App.Entities.Dtos.CategoryDtos;
using App.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin,Editor")]

	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public async Task<IActionResult> Index()
		{
			var result = await _categoryService.GetAllAsync();
			return View(result);
		}
	
		public async Task<IActionResult> Create()
		{
			var category = await _categoryService.GetAllAsync();
			ViewBag.CategoryId = new SelectList(category.Data.Categories, "Id", "Name");
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CategoryAddDto categoryAddDto)
		{
			if (categoryAddDto is null)
			{
				return RedirectToAction(nameof(Index));
			}

			if (ModelState.IsValid)
			{


				await _categoryService.AddAsync(categoryAddDto, "Admin");
				return RedirectToAction(nameof(Index));

			}
			return View(categoryAddDto);
		}

	
		public async Task<IActionResult> Edit(int id)
		{
			var model = await _categoryService.GetCategoryUpdateDtoAsync(id);
			var category = await _categoryService.GetAllAsync();
			ViewBag.CategoryId = new SelectList(category.Data.Categories, "Id", "Name");
			return View(model.Data);
		}

	
		[HttpPost]
		public async Task<IActionResult> Edit(CategoryUpdateDto categoryUpdateDto)
		{
			if (ModelState.IsValid)
			{

				await _categoryService.UpdateAsync(categoryUpdateDto, "Admin");
				return RedirectToAction(nameof(Index));

			}
			return View(categoryUpdateDto);
		}
	
		public async Task<IActionResult> Delete(int id)
		{
	
			await _categoryService.DeleteAsync(id, "Emre");
			return RedirectToAction(nameof(Index));
		}

	}
}
