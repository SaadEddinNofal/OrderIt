using OrderITDemo.ViewModels;

namespace OrderITDemo.Abstraction
{
    public interface ICartService
    {
        Task<CartViewModel> GetUserCartAsync(string userId);
        Task AddToCartAsync(int menuId, string userId);
        Task DecreaseItemQuantityAsync(int menuId, string userId);
        void DeleteCartItem(int id);
    }
}
