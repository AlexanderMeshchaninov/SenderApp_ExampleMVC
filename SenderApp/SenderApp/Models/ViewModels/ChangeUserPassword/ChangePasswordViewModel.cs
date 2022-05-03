using Core.Abstractions.ViewModels;

namespace SenderApp.Models.ViewModels.ChangeUserPassword
{
    public class ChangePasswordViewModel : AbstractChangePasswordBaseViewModel
    {
        public override string Email { get; set; }
        public override string NewPassword { get; set; }
        public override string OldPassword { get; set; }
    }
}