using Core.Abstractions.ViewModels;

namespace SenderApp.Models.ViewModels.EditUserInfo
{
    public class EditUserViewModel : AbstractEditBaseViewModel
    {
        public override string Email { get; set; }
    }
}