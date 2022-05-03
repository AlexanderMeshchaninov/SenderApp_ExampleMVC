using System.Threading.Tasks;
using Core.Abstractions.Gateway;

namespace SenderApp.Services.MessageService
{
    public class SmsService : IChannel
    {
        public Task SendMessageAsync(
            string currentUserName, 
            string email, 
            string subject, 
            string message)
        {
            //SmsService logic...
            
            return Task.CompletedTask;
        }
    }
}