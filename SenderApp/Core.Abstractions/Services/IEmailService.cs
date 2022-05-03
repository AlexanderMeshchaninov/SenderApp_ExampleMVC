using System.Threading.Tasks;

namespace Core.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendMessageAsync(
            string email, 
            string subject, 
            string message);
    }
}