using SenderApp.Models.ViewModels;
using SenderApp.Models.ViewModels.EditUserInfo;

namespace SenderApp.Models.DTO.Responses
{
    public sealed class ResponseEditUserDto : EditUserViewModel
    {
        public override string Email { get; set; }
    }
}