using System.Threading.Tasks;
using Core.Abstractions.Gateway;
using MailKit.Net.Smtp;
using MimeKit;

namespace SenderApp.Services.MessageService
{
    public class EmailService : IChannel
    {
        public async Task SendMessageAsync(
            string currentUserName, 
            string email, 
            string subject, 
            string message)
        {
            var emailMessage = new MimeMessage();
 
            emailMessage.From.Add(new MailboxAddress("SenderApp", "your_real_email@mail.ru"));
            emailMessage.To.Add(new MailboxAddress($"Dear, {currentUserName}", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, true);
                await client.AuthenticateAsync("your_real_email@mail.ru", "your_email_password");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}