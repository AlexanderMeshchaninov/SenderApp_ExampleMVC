using System.Threading.Tasks;
using Core.Abstractions.Gateway;

namespace SenderApp.Services.MessageService
{
    public class PushService : IChannel
    {
        public Task SendMessageAsync(
            string currentUserName, 
            string email, 
            string subject, 
            string message)
        {
            //PushService logic...
            
            return Task.CompletedTask;
        }
    }
}