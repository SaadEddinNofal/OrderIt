using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderITDemo.Models
{
    [NotMapped]
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public int CityId { get; set; }
        public string? Street { get; set; }                               
        public int UserId { get; set; }           
        public  AppUser User { get; set; }
        public  City City { get; set; }
    }
}
