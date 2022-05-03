using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.ViewModels
{
    public abstract class AbstractChangeRoleBaseModel
    {
        [Required]
        public abstract string UserId { get; set; }
        
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public abstract string UserEmail { get; set; }
    }
}