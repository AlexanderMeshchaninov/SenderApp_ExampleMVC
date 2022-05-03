using Core.Abstractions.ViewModels;

namespace SenderApp.Models.ViewModels.LoginUser
{
    public class LoginViewModel : AbstractLoginBaseViewModel
    {
        public override string Email { get; set; }
        public override string Password { get; set; }
        public override bool RememberMe { get; set; }
        public override string ReturnUrl { get; set; }
    }
}