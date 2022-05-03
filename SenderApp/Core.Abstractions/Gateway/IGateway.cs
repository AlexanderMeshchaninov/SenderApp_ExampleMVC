using System.Threading.Tasks;

namespace Core.Abstractions.Gateway
{
    public interface IGateway
    {
        public Task SetGateAsync(IChannel channel);
        public Task SendMessageAsync(
            string currentUserName, 
            string email, 
            string subject, 
            string message);
    }
}