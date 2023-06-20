using App.Entities.Concrete;
using App.Entities.Dtos.UserDtos;
using App.Web.Mvc.Helpers.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize]
	public class SettingController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;
        private readonly IImageHelper _imageHelper;
        private readonly SignInManager<User> _signInManager;

        public SettingController(UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _imageHelper = imageHelper;
            _signInManager = signInManager;
        }
    
        public async Task<IActionResult> UserChangeDetails()
		{
			var user= await _userManager.GetUserAsync(HttpContext.User);
			var updateDto = _mapper.Map<UserUpdateDto>(user);
			return View(updateDto);
		}

        [HttpPost]
        public async Task<IActionResult> UserChangeDetails(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool newImageUploaded = false;
                var oldUser = await _userManager.GetUserAsync(HttpContext.User);
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
                    TempData.Add("Message", "Your user settings has been successfully updated.");
                    return View(userUpdateDto);

                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(userUpdateDto);
                }
            }
            return View(userUpdateDto);
        }

        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(UserPasswordChangeDto userPasswordChangeDto)     
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var isVerified = await _userManager.CheckPasswordAsync(user, userPasswordChangeDto.CurrentPassword); //Şuanki şifre doğru mu?
                if (isVerified)
                {
                    var result = await _userManager.ChangePasswordAsync(user, userPasswordChangeDto.CurrentPassword, userPasswordChangeDto.NewPassword);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user); // Kullanıcının önemli bir bilgisinin değiştiği gösterir
                        await _signInManager.SignOutAsync(); // Kullanıcıyı çıkış yaptırıp yeni şifre ile girmesini sağlamalıyız					
						await _signInManager.PasswordSignInAsync(user, userPasswordChangeDto.NewPassword, true, false);
						await _userManager.UpdateSecurityStampAsync(user);
						TempData.Add("Message", "Your password has been changed successfully.");
                        return View();
                    }
                     foreach(var error in result.Errors)
                     {
                            ModelState.AddModelError("", error.Description);
                     }
                     return View(userPasswordChangeDto);
                    
                }
                ModelState.AddModelError("", "Please check your password.");
                return View(userPasswordChangeDto);
            }
            return View(userPasswordChangeDto);
        }

    }
}
