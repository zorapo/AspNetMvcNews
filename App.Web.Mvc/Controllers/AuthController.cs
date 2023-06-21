using App.Entities.Concrete;
using App.Entities.Dtos;
using App.Entities.Dtos.UserDtos;
using App.Service.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Controllers
{
	public class AuthController : Controller
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly ISendEmailService _sendEmailService;
		private readonly IMapper _mapper;

		public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, ISendEmailService sendEmailService)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
			_mapper = mapper;
			_sendEmailService = sendEmailService;
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
		{
			
			if (ModelState.IsValid)
			{
				var newUser = _mapper.Map<User>(userRegisterDto);
				newUser.Picture = "default.jpg";	
				
				var result = await _userManager.CreateAsync(newUser, userRegisterDto.Password); //Şifreyi hashleme işlemi yapıyor

				if (result.Succeeded) //IdentityResult kütüphaneden geliyor
				{
					await _userManager.AddToRoleAsync(newUser, "User");
					TempData.Add("MessageAuth", "Your registration has been successfully.");					
					return View(nameof(Register));
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
					return View(userRegisterDto);
				}

			}

			return View(userRegisterDto);

		}
		public IActionResult Login()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(UserLoginDto userLoginDto, string? returnUrl = null)
		{
			returnUrl = returnUrl ?? Url.Action("Index", "Home");
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
				if (user == null)
				{
					ModelState.AddModelError("", "E-Posta adresiniz veya şifreniz hatalı.");
					return View(userLoginDto);
				}

				var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe, true);
				if (result.Succeeded)
				{
					return Redirect(returnUrl);
				}
				if (result.IsLockedOut)
				{
					var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user); // hesabın kilitli kalacağı zamanı elde ediyor.
					var timeLeft = lockoutEnd.Value - DateTime.Now; // Bu ise kilitlendikten sonra açılmasına kaç dk kaldığını gösteriyor.						

					ModelState.AddModelError("", $@"Şifrenizi 3 kere hatalı girdiniz. Hesabınız geçici olarak kilitlenmiştir. 
													Lütfen {timeLeft.Minutes} dakika sonra tekrar deneyin.");
					return View();
				}

				ModelState.AddModelError("", "E-Posta adresiniz veya şifreniz hatalı.");
				return View(userLoginDto);

			}
			ModelState.AddModelError("", $"E-Posta adresiniz veya şifreniz hatalı.");
			return View(userLoginDto);
		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home", new { Area = "" });
		}
		public IActionResult ForgotPassword()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
		{
			var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "Bu E-posta adresine sahip kullanıcı bulunamamıştır.");
				return View();
			}
			string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
			var passwordResetLink = Url.Action("ResetPassword", "Auth", new
			{
				userId = user.Id,
				token = passwordResetToken
			}, HttpContext.Request.Scheme);
			await _sendEmailService.SendResetPasswordEmail(passwordResetLink, user.Email);
			TempData["MessageAuth"] = "Şifre yenileme linki e-posta adresinize gönderilmiştir.";

			return RedirectToAction(nameof(ForgotPassword));
		}
		public IActionResult ResetPassword(string userId, string token)
		{

			return token == null ? View("ErrorPage") : View(new ResetPasswordDto
			{
				TokenCode = token,
			});  

		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
				if (user == null)
				{
					ModelState.AddModelError("Email", "Kullanıcı bulunamadı.");
					return View();
				}
				var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.TokenCode, resetPasswordDto.Password);
				if (result.Succeeded)
				{
					TempData["MessageAuth"] = "Şifreniz başarıyla yenilenmiştir.";
					await _userManager.UpdateSecurityStampAsync(user);
					return View(resetPasswordDto);
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
					return View(resetPasswordDto);
				}

			}
			return View(resetPasswordDto);
		}

		public IActionResult AccessDenied()
		{

			return View();
		}
	}
}
