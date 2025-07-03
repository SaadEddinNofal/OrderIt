using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderITDemo.Models
{
    //[NotMapped]
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("Menu")]
        public int MenuId { get; set; }
        public int? OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? MenuName { get; set; }

        public virtual  Menu Menu { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Order Order { get; set; }
    }
}
