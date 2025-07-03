using System.ComponentModel.DataAnnotations;

namespace OrderITDemo.ViewModels
{
    public class RoleFormViewModel
    {
        [Required , StringLength(256)]
        public string? Name { get; set; }
    }
}
