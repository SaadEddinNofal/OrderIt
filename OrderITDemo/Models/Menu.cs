using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderITDemo.Models
{
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }                    
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;             
        public string? Description { get; set; }        
        public decimal Price { get; set; }
        [NotMapped]
        public IFormFile? MenuFile { get; set; } // it well get the file from the user who is using the site
        public byte[]? ItemPicture { get; set; }
        public bool IsExist { set; get; }
        public virtual Category? Category { get; set; }
    }
}
