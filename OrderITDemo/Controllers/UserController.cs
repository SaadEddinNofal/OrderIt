using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderITDemo.Models;
using OrderITDemo.ViewModels;
using System.Net.Mail;
using System.Runtime.InteropServices;

namespace OrderITDemo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManger;
        public UserController(RoleManager<IdentityRole> roleManger, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _roleManger = roleManger;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                                          .Select(user => new
                                          {
                                              user.Id,
                                              user.FirstName,
                                              user.PhoneNumber,
                                              user.Email,
                                              user.UserName
                                          })
                                          .ToListAsync();

            // إنشاء قائمة من UserViewModel مع الأدوار  
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                // جلب الأدوار بشكل غير متزامن  
                var roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));

                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = roles
                });
            }

            return View(userViewModels);
        }

        public async Task<IActionResult> Add()
        {
            var roles = await _roleManger.Roles.Select(r=>new RoleViewModel {RoleId=r.Id , RoleName = r.Name }).ToListAsync();
            var viewModel = new AddUserViewModel
            {
                Roles = roles.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!model.Roles.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Roles" ,"Please select at least one role");
                return View(model);
            }
            if(await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email","Email is already exist ");
                return View(model);
            }
            if (await _userManager.FindByEmailAsync(model.UserName) != null)
            {
                ModelState.AddModelError("UserName", "User Name is already exist ");
                return View(model);
            }
            var user = new AppUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };



            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Roles", error.Description);
                }
                return View(model);
            }
            await _userManager.AddToRolesAsync(user , model.Roles.Where(r=>r.IsSelected).Select(r=>r.RoleName));

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            // we done the cheack if we have user or not
            var viewModel = new FileProfileViewModel
            {
                Id=user.Id,
                FirstName = user.FirstName,
                PhoneNumber = user.PhoneNumber,
               UserName=user.UserName,
               Email = user.Email
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FileProfileViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(model);
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail != null && userWithSameEmail.Id != model.Id) 
            {
                ModelState.AddModelError("Email" , "This email is already used");
                return View(model);
            }
            var userWithUserName = await _userManager.FindByNameAsync(model.UserName);
            if (userWithUserName != null && userWithUserName.UserName != model.UserName)
            {
                ModelState.AddModelError("UserName", "This UserName is already used");
                return View(model);
            }
            user.FirstName = model.FirstName;
            user.PhoneNumber  = model.PhoneNumber;
            user.UserName = model.UserName;
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound();
            }
            var roles = await _roleManger.Roles.ToListAsync();
            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName=role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user,role.Name).Result
                })
                .ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync (model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected) // selected but then not selected
                {
                    await _userManager.RemoveFromRoleAsync(user , role.RoleName);
                }
                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected) // not selected then selected
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
