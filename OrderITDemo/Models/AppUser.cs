using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OrderITDemo.Models
{
    public class AppUser:IdentityUser
    {
        
        [Required , MaxLength(20)]
        public string? FirstName { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public  ICollection<CartItem>? CartItems { get; set; }
     /*   public int AdressId { get; set; }
        public Address Address { get; set; }*/
    }

}
