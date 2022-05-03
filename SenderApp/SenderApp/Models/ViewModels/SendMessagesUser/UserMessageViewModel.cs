using System;

namespace SenderApp.Models.ViewModels.SendMessagesUser
{
    public class UserMessageViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Template { get; set; }
        public string Message { get; set; }
        public DateTime SendingMessageTime { get; set; }
        public bool IsSendingMessageBySpecificTime { get; set; }
    }
}