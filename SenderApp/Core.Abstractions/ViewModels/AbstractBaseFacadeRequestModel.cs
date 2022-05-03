using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.ViewModels
{
    public abstract class AbstractBaseFacadeRequestModel
    {
        public string Id { get; set; }
        
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public abstract string Email { get; set; }
        
        [StringLength(100, ErrorMessage = "Password length min - 5", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public abstract string Password { get; set; }
        
        [StringLength(100, ErrorMessage = "Password length min - 5", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public abstract string NewPassword { get; set; }
        
        [StringLength(100, ErrorMessage = "Password length min - 5", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public abstract string OldPassword { get; set; }
        
        [StringLength(100, ErrorMessage = "Password length min - 5", MinimumLength = 5)]
        [Compare("Password", ErrorMessage = "Password mismatch")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public abstract string PasswordConfirm { get; set; }
        
        [Display(Name = "Remember me?")]
        public abstract bool RememberMe { get; set; }
        public abstract string ReturnUrl { get; set; }
    }
}