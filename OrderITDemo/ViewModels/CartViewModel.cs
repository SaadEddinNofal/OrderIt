using OrderITDemo.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderITDemo.ViewModels
{
    public class CartViewModel
    {
        public ICollection<CartItem>? Carts { get; set; }
        [Required]
        [StringLength(maximumLength: 200, MinimumLength = 3 , ErrorMessage ="you should enter {2} at least and {1} max Chars ")]

        public string Address { get; set; } = "";
    }
}
