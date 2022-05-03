using Core.Abstractions.ViewModels;

namespace SenderApp.Models.ViewModels.EditUserInfo
{
    public class CreateUserViewModel : AbstractCreateBaseViewModel
    {
        public override string Email { get; set; }
        public override string Password { get; set; }
    }
}