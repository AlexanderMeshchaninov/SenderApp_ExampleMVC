namespace SenderApp.Models.DTO.Requests
{
    public sealed class RequestTemplateDto
    {
        public string Email { get; set; }
        public string CallBackUrl { get; set; }
        public string Template { get; set; }
        public string Message { get; set; }
    }
}