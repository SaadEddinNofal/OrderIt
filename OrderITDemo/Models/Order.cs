using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderITDemo.Models
{
   
    public class Order
    {
        [Key]
        public int OrderId { get; set; }                
        public string UserId { get; set; }                          
        public decimal? TotalAmount { get; set; }
        public OrderStatus OrderStatuss { get; set; }     
        public DateTime StartOrderDate { get; set; }
        public DateTime StartDeliveryDate { get; set; }
        public DateTime EndOrderDate { get; set; }
        [StringLength(maximumLength: 200, MinimumLength = 3, ErrorMessage = "you should enter {2} at least and {1} max Chars ")]
        public string? Address { get; set; }
        public string? DeliveryId { get; set; }
        public  AppUser? User { get; set; }
        public virtual AppUser? Delivery { get; set; }
        public  ICollection<CartItem>? Carts { get; set; }
        public ICollection<OrderItems>? Orders { get; set; }


        public enum OrderStatus
        {
            Pendeing,
            Processing,   
            UnderDelivery,
            Delivered,    
            Cancelled     
        }
    }
}
