using App.Entities.Concrete;
using App.Entities.Dtos.UserDtos;
using App.Web.Mvc.Helpers.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace App.Web.Mvc.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]

	public class UserController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly IImageHelper _imageHelper;
		private readonly IMapper _mapper;
		private readonly RoleManager<Role> _roleManager;
		public UserController(UserManager<User> userManager, IImageHelper imageHelper, IMapper mapper, RoleManager<Role> roleManager)
		{
			_userManager = userManager;
			_imageHelper = imageHelper;
			_mapper = mapper;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users.ToListAsync();
		    

			return View(new UserListDto
			{
				Users = users

			});
		}
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(UserAddDto userAddDto)
		{
			if (ModelState.IsValid)
			{
				if (userAddDto.PictureFile == null)
				{
					userAddDto.Picture = "default.jpg";
				}
				else
				{
				userAddDto.Picture = await _imageHelper.ImageUpload(userAddDto.UserName, userAddDto.PictureFile, "user");
				}
				
				var user = _mapper.Map<User>(userAddDto);			
				var result = await _userManager.CreateAsync(user, userAddDto.Password); //Şifreyi hashleme işlemi yapıyor

				if (result.Succeeded) //IdentityResult kütüphaneden geliyor
				{
					await _userManager.AddToRoleAsync(user, "User");
					return RedirectToAction(nameof(Index));
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
					return View(userAddDto);
				}

			}

			return View(userAddDto);
		}

		public async Task<IActionResult> Delete(string userId)
		{
           
            var user = await _userManager.FindByIdAsync(userId);
			
			if (user == null)
			{
				ViewBag.ErrorMessage = $"{userId}'li kullanıcı bulunamadı.";
				return NotFound();
			}
			else
			{
				try
				{
                    if (user.Picture != "default.jpg")
                    {
                        
                       await _imageHelper.ImageDelete(user.Picture, "user");
                    }
					var result = await _userManager.DeleteAsync(user);

					if (result.Succeeded)
					{
						return RedirectToAction("Index");
					}

					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
					return View("Index");
				}
				catch (Exception)
				{
					ViewBag.ErrorTitle = $"{user.UserName} kullanıcısı silinememektedir!";
					ViewBag.ErrorMessage = $"{user.UserName} kullanıcısı bir role sahip olduğu için silinememektedir. Bu kullanıcıyı silmek istiyorsanız, kullanıcının rolünü kaldırın ve sonra tekrar deneyin.";
					return View("ErrorPage");
				}

			}

		}

		public async Task<IActionResult> Edit(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			var userUpdateDto = _mapper.Map<UserUpdateDto>(user);

			return View(userUpdateDto);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(UserUpdateDto userUpdateDto)
		{
			if (ModelState.IsValid)
			{
				bool newImageUploaded = false;
				var oldUser = await _userManager.FindByIdAsync(userUpdateDto.Id);
				var oldUserPicture = oldUser.Picture;
				if (userUpdateDto.PictureFile != null)
				{
					userUpdateDto.Picture = await _imageHelper.ImageUpload(userUpdateDto.UserName, userUpdateDto.PictureFile, "user");
                    if (oldUserPicture != "default.jpg")
                    {
                        newImageUploaded = true;
                    }
                }
				var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser); //UserUpdateDto'u varolan User modeline dönüştürüyor
				var result = await _userManager.UpdateAsync(updatedUser);
				if (result.Succeeded)
				{
					await _userManager.UpdateSecurityStampAsync(updatedUser);

					if (newImageUploaded)
					{
						await _imageHelper.ImageDelete(oldUserPicture, "user");
					}
					return RedirectToAction(nameof(Index));

				}
				else
				{

					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
					return View(userUpdateDto);
				}
			}
			return View(userUpdateDto);


		}

	}
}
