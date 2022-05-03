using System.Threading.Tasks;
using Core.Abstractions.Gateway;

namespace SenderApp.CommonLogic.Gateway
{
    public sealed class Gateway : IGateway
    {
        private IChannel _channel;

        public Gateway()
        { }

        public Gateway(IChannel channel)
        {
            _channel = channel;
        }

        public Task SetGateAsync(IChannel channel)
        {
            _channel = channel;
            return Task.CompletedTask;
        }

        public async Task SendMessageAsync(
            string currentUserName, 
            string email, 
            string subject, 
            string message)
        {
            await _channel.SendMessageAsync(currentUserName, email, subject, message);
        }
    }
}