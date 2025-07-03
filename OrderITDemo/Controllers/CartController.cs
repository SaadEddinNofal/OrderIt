using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using OrderITDemo.Abstraction;
using OrderITDemo.Data;
using OrderITDemo.Models;
using OrderITDemo.Repository.Base;
using OrderITDemo.ViewModels;
using System.Runtime.InteropServices;

namespace OrderITDemo.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<AppUser> _userManager;

        public CartController(ICartService cartService, UserManager<AppUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var cart = await _cartService.GetUserCartAsync(user.Id);
            if (cart.Carts == null) return NotFound();
            return View(cart.Carts.Count == 0 ? null : cart);
        }

        public async Task<IActionResult> AddToCart(int menuId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            await _cartService.AddToCartAsync(menuId, user.Id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int menuId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            await _cartService.DecreaseItemQuantityAsync(menuId, user.Id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            _cartService.DeleteCartItem(id);
            return RedirectToAction(nameof(Index));
        }
    }

}

