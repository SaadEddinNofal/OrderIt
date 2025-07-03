using Microsoft.Build.Execution;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderITDemo.Models
{
  
    public class OrderItems
    {
        [Key]
        public int InvoiceId { get; set; }          
        public int OrderId { get; set; }
        public int Quentity { get; set; }
        public string MenuName { get; set; }
        public decimal? Price { get; set; }               

        public virtual Order Order { get; set; }
        [NotMapped]
        public virtual CartItem CartItem { set; get; }
    }
}
