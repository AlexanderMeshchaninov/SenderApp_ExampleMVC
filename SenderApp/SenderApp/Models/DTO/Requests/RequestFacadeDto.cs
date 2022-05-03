using Core.Abstractions.ViewModels;

namespace SenderApp.Models.DTO.Requests
{
    public sealed class RequestFacadeDto : AbstractBaseFacadeRequestModel
    {
        public override string Email { get; set; }
        public override string Password { get; set; }
        public override string NewPassword { get; set; }
        public override string OldPassword { get; set; }
        public override string PasswordConfirm { get; set; }
        public override bool RememberMe { get; set; }
        public override string ReturnUrl { get; set; }
    }
}