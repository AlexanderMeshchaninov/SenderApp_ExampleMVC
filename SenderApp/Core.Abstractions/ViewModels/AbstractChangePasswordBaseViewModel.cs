using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.ViewModels
{
    public abstract class AbstractChangePasswordBaseViewModel
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public abstract string Email { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Password length min - 5", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public abstract string NewPassword { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Password length min - 5", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public abstract string OldPassword { get; set; }
    }
}