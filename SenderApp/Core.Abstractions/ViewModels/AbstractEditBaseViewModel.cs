using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.ViewModels
{
    public abstract class AbstractEditBaseViewModel
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public abstract string Email { get; set; }
    }
}