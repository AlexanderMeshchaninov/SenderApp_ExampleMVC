using System.Threading.Tasks;
using Quartz;
using SenderApp.CommonLogic.Gateway;
using SenderApp.Services.MessageService;

namespace SenderApp.Services.QuartzService.Jobs
{
    public sealed class SendEmailJob : IJob
    {
        private readonly Gateway _gateway = new Gateway();

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _gateway.SetGateAsync(new EmailService());
            
                var dataMap = context.JobDetail.JobDataMap;
            
                var email = dataMap.GetString("Email");
                var message = dataMap.GetString("Message");
                var templateSubject = dataMap.GetString("TemplateSubject");
                var currentUserName = dataMap.GetString("CurrentUserName");

                await _gateway.SendMessageAsync(
                    currentUserName, 
                    email, 
                    templateSubject, 
                    message);

                await Task.CompletedTask;
            }
            catch (JobExecutionException ex)
            {
                await Task.FromResult(ex);
            }
        }
    }
}