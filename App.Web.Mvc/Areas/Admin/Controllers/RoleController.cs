using App.Entities.Concrete;
using App.Entities.Dtos;
using App.Entities.Dtos.RoleDtos;
using App.Web.Mvc.Areas.Admin.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Web.Mvc.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class RoleController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly IMapper _mapper;

		public RoleController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			var roles = _roleManager.Roles.ToList();
			return View(new RoleListDto
			{
				Roles = roles
			});
		}
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(RoleAddDto roleAddDto)
		{
			if (ModelState.IsValid)
			{
				var role = _mapper.Map<Role>(roleAddDto);
				var result = await _roleManager.CreateAsync(role);

				if (result.Succeeded) //IdentityResult kütüphaneden geliyor
				{
					return RedirectToAction(nameof(Index));
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
					return View(roleAddDto);
				}

			}

			return View(roleAddDto);
		}

		public async Task<IActionResult> Delete(string roleId)
		{

			var role = await _roleManager.FindByIdAsync(roleId);

			if (role == null)
			{
				ViewBag.ErrorMessage = $"{roleId}'li rol bulunamadı.";
				return NotFound();
			}
			else
			{
				try
				{
					var result = await _roleManager.DeleteAsync(role);

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
				catch (DbUpdateException) // DbUpdateException database'e bir şey kaydederken ortaya çıkan sorunları yakalayıp hata fırlatıyor.
				{
					ViewBag.ErrorTitle = $"{role.Name} rolü kullanımdadır.";
					ViewBag.ErrorMessage = $"{role.Name} rolünde kullanıcılar bulunduğu için silinememektedir. Bu rolü silmek istiyorsanız, bu roldeki kullanıcıları kaldırın ve sonra tekrar deneyin.";
					return View("ErrorPage");
				}
			}

		}
		public async Task<IActionResult> Edit(string roleId)
		{
			var role = await _roleManager.FindByIdAsync(roleId);
			var roleUpdateDto = _mapper.Map<RoleUpdateDto>(role);

			return View(roleUpdateDto);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(RoleUpdateDto roleUpdateDto)
		{
			if (ModelState.IsValid)
			{
				var role = await _roleManager.FindByIdAsync(roleUpdateDto.Id);

				var updatedRole = _mapper.Map<RoleUpdateDto, Role>(roleUpdateDto, role);
				var result = await _roleManager.UpdateAsync(updatedRole);

				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Index));
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return View(roleUpdateDto);

			}
			return View(roleUpdateDto);
		}
		public async Task<IActionResult> AssignUserRole(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			TempData["UserId"] = user.Id;
			var roles = await _roleManager.Roles.ToListAsync();
			var userRoles = await _userManager.GetRolesAsync(user);
			var userWithRolesDto = new UserWithRolesDto
			{
				UserId = user.Id,
				UserName = user.UserName
			};

			foreach (var role in roles)
			{
				var assignRoleToUserDto = new AssignRoleToUserDto()
				{
					RoleId = role.Id,
					RoleName = role.Name,
					HasRole=userRoles.Contains(role.Name)
				};
				
				userWithRolesDto.AssignRoleToUserDtos.Add(assignRoleToUserDto);
			}
			return View(userWithRolesDto);


		}

		[HttpPost]
		public async Task<IActionResult> AssignUserRole(UserWithRolesDto userWithRolesDto)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(userWithRolesDto.UserId);


				foreach (var role in userWithRolesDto.AssignRoleToUserDtos)
				{
					if (role.HasRole)
					{
						await _userManager.AddToRoleAsync(user, role.RoleName);
					}
					else
					{
						await _userManager.RemoveFromRoleAsync(user, role.RoleName);
					}

				}
				await _userManager.UpdateSecurityStampAsync(user); //Rol atamadan sonra SecurityStamp değerini update et.
			}
			return RedirectToAction("Index","User");

		}
	}
}
