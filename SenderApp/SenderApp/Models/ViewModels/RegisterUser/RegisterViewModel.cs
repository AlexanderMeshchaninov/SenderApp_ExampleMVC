using Core.Abstractions.ViewModels;

namespace SenderApp.Models.ViewModels.RegisterUser
{
    public class RegisterViewModel : AbstractRegisterBaseViewModel
    {
        public override string Email { get; set; }
        public override string Password { get; set; }
        public override string PasswordConfirm { get; set; }
    }
}