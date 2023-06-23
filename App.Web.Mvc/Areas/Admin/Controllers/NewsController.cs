using App.Entities.Concrete;
using App.Entities.Dtos.NewsDtos;
using App.Service.Abstract;
using App.Shared.Utilities.Results.ComplexTypes;
using App.Web.Mvc.Helpers.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Web.Mvc.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Editor")]

	public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;  
        private readonly UserManager<User> _userManager;
        private readonly IImageHelper _imageHelper;
        private readonly IMapper _mapper;
        private readonly ILogger<NewsController> _logger;

		public NewsController(INewsService newsService, ILogger<NewsController> logger, ICategoryService categoryService, IImageHelper imageHelper, UserManager<User> userManager, IMapper mapper)
		{
			_newsService = newsService;
			_logger = logger;
			_categoryService = categoryService;
			_imageHelper = imageHelper;
			_userManager = userManager;
			_mapper = mapper;
	
		}
		public async Task<IActionResult> Index()
        {
            var model = await _newsService.GetAllAsync();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var model = _newsService.GetAsync(id);
            return View(model);
        }


        public async Task<IActionResult> Create()
        {
            var category = await _categoryService.GetAllAsync();
            var user = _userManager.Users.ToList();
            ViewBag.CategoryId = new SelectList(category.Data.Categories, "Id", "Name");
			ViewBag.USerId = new SelectList(user, "Id", "UserName");
			return View();
        }

 
        [HttpPost]
        public async Task<IActionResult> Create(NewsAddDto newsAddDto)
        {
            if(newsAddDto is null)
            {
                return RedirectToAction(nameof(Index));
            }
          
			if (ModelState.IsValid)
			{
			        newsAddDto.ImagePath = await _imageHelper.ImageUpload("news", newsAddDto.ImageFile, "news");

					await _newsService.AddAsync(newsAddDto, "Admin");
					return RedirectToAction(nameof(Index));

			}

            return View(newsAddDto);
        }
        public async Task<IActionResult> Edit(int newsId)
        {
            
            var model = await _newsService.GetUpdateDtoAsync(newsId);
            var user = _userManager.Users.ToList();
            var category = await _categoryService.GetAllAsync();
            ViewBag.CategoryId = new SelectList(category.Data.Categories, "Id", "Name");
            ViewBag.USerId = new SelectList(user, "Id", "UserName");
            return View(model.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NewsUpdateDto newsUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool newImageUploaded = false;
                var oldNews = await _newsService.GetAsync(newsUpdateDto.Id);
                var oldNewsPicture = oldNews.Data.News.ImagePath;
                if (newsUpdateDto.ImageFile != null)
                {
                    newsUpdateDto.ImagePath = await _imageHelper.ImageUpload("news", newsUpdateDto.ImageFile, "news");
                    newImageUploaded = true;
                }
                var result = await _newsService.UpdateAsync(newsUpdateDto,"Admin");
                if (result.ResultStatus==ResultStatus.Success)
                {
                    if (newImageUploaded)
                    {
                        await _imageHelper.ImageDelete(oldNewsPicture, "news");
                    }
                    return RedirectToAction(nameof(Index));

                }
               return RedirectToAction(nameof(Index));

            }
         
            var user = _userManager.Users.ToList();
            var category = await _categoryService.GetAllAsync();
            ViewBag.CategoryId = new SelectList(category.Data.Categories, "Id", "Name");
            ViewBag.USerId = new SelectList(user, "Id", "UserName");
            return View(newsUpdateDto);
            
        }

        public async Task<IActionResult> Delete(int newsId)
        {

            await _newsService.DeleteAsync(newsId,"Emre");
            return RedirectToAction(nameof(Index));

        }

		public async Task<IActionResult> Detail(int newsId)
		{

			var news=await _newsService.GetAsync(newsId);

			return View(news);

		}



	}
}
