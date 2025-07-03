using System.ComponentModel.DataAnnotations;

namespace OrderITDemo.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }        
        public string Name { get; set; }           
        public string? Description { get; set; }    

        public  ICollection<Menu>? Menu { get; set; }
    }
}
