using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.ViewModels
{
    public abstract class AbstractCreateBaseViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public abstract string Email { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Password length min - 5", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public abstract string Password { get; set; }
    }
}