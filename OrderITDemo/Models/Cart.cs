using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderITDemo.Models
{
    [NotMapped]
    public class Cart
    {
        [Key]
        public int CartId { get; set; }             // معرف السلة  
        public Guid UserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public  AppUser User { get; set; }      // الربط بالمستخدم  
    
        public  ICollection<CartItem> CartItems { get; set; }
    }
}
