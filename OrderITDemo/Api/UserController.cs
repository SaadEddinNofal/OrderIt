using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderITDemo.Models;

namespace OrderITDemo.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public UserController(RoleManager<IdentityRole> roleManger, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
            {
                return NotFound();
            }
           var res= await _userManager.DeleteAsync(user);
            if (!res.Succeeded) { throw new Exception(); }
            return Ok();
        }
    }
}
