using Microsoft.EntityFrameworkCore;
using OrderITDemo.Abstraction;
using OrderITDemo.Models;
using OrderITDemo.Repository.Base;
using OrderITDemo.ViewModels;

namespace OrderITDemo.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<CartItem> _cartItemRepo;
        private readonly IRepository<Menu> _menuRepo;
        private readonly IEnumerable<ICartDiscount> _discounts;
        public CartService(IRepository<CartItem> cartItemRepo, IRepository<Menu> menuRepo, IEnumerable<ICartDiscount> discounts)
        {
            _cartItemRepo = cartItemRepo;
            _menuRepo = menuRepo;
            _discounts = discounts;
        }

        public async Task<CartViewModel> GetUserCartAsync(string userId)
        {
            var cartItems = await _cartItemRepo.GetAll().Where(c => c.UserId == userId)
            .Include(c => c.Menu)
            .ToListAsync();

            return new CartViewModel
            {
                Carts = cartItems,
                Address = ""
            };
        }
        public decimal CalculateTotalWithDiscount(decimal total, string userId)
        {
            foreach (var discount in _discounts)
                total = discount.ApplyDiscount(total, userId);
            return total;
        }

        public async Task AddToCartAsync(int menuId, string userId)
        {
            var existing = _cartItemRepo
                .GetAll()
                .FirstOrDefault(c => c.MenuId == menuId && c.UserId == userId);

            var menu = await _menuRepo.GetByIdAsync(menuId);
            if (menu == null) throw new Exception("Menu not found");

            if (existing != null)
            {
                existing.Quantity++;
                existing.TotalPrice = existing.Quantity * menu.Price;
                _cartItemRepo.Update(existing);
            }
            else
            {
                var item = new CartItem
                {
                    MenuId = menuId,
                    MenuName = menu.Name,
                    Quantity = 1,
                    TotalPrice = menu.Price,
                    UserId = userId
                };
                _cartItemRepo.Add(item);
            }
            _cartItemRepo.Save();
        }

        public async Task DecreaseItemQuantityAsync(int menuId, string userId)
        {
            var item = _cartItemRepo
                .GetAll()
                .FirstOrDefault(c => c.MenuId == menuId && c.UserId == userId);

            if (item == null) return;

            item.Quantity--;
            if (item.Quantity <= 0)
            {
                _cartItemRepo.Delete(item.MenuId);
            }
            else
            {
                var menu = await _menuRepo.GetByIdAsync(menuId);
                item.TotalPrice = item.Quantity * menu.Price;
                _cartItemRepo.Update(item);
            }

            _cartItemRepo.Save();
        }

        public void DeleteCartItem(int id)
        {
            _cartItemRepo.Delete(id);
            _cartItemRepo.Save();
        }
    }

}
public interface ICartDiscount
{
    decimal ApplyDiscount(decimal total, string userId);
}