using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderITDemo.Models
{
    [NotMapped]
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }            // معرف عنصر الطلب  
        public int OrderId { get; set; }                 // معرف الطلب  
        public int MenuId { get; set; }              // معرف عنصر القائمة  
        public int Quantity { get; set; }                 // كمية العنصر المطلوبة  

        public virtual Order Order { get; set; }          // الربط بالطلب  
        public virtual Menu Menu { get; set; }
    }
}
