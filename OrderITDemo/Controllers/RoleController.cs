using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderITDemo.ViewModels;

namespace OrderITDemo.Controllers;

[Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<IActionResult> Index()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(RoleFormViewModel model)
    {
        /* if (ModelState.!IsValid)
             return View("Index", await _roleManager.Roles.ToListAsync());*/
        if (model.Name != null)
        {
            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "Role is exist! Try another one!");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim())); // Trime Delete the spaces in strings
            return RedirectToAction(nameof(Index)); // this way is better cause if u change the name of action it will give u an exepction
        }
        else
        {
            ModelState.AddModelError("Name", "Enter A name Please");
            return View("Index", await _roleManager.Roles.ToListAsync());
        }

    }
    public async Task<IActionResult> Delete(string id)
    {
        var role = _roleManager.FindByIdAsync(id).Result;
        if (role == null)
        {
            return NotFound();
        }
        await _roleManager.DeleteAsync(role);
        return RedirectToAction(nameof(Index));
    }
}
