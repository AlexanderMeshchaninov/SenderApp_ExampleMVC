using System.Threading.Tasks;

namespace Core.Abstractions.Gateway
{
    public interface IChannel
    {
        Task SendMessageAsync(
            string currentUserName,
            string email, 
            string subject, 
            string message);
    }
}