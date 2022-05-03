using System;
using System.Threading.Tasks;
using Core.Abstractions.Services;
using Quartz;
using SenderApp.Services.QuartzService.Jobs;

namespace SenderApp.Services.QuartzService.Schedulers
{
    public class SendEmailScheduler : ISendScheduler
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public SendEmailScheduler(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task Run(
            string currentUserName, 
            string email, 
            string message, 
            string templateSubject, 
            DateTime timeToStart)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var num = DateTime.Now.ToString("yymmssfff");
            
            var job = JobBuilder.Create<SendEmailJob>()
                .WithIdentity("emailsendjob", $"groupEmail{num}")
                .UsingJobData("CurrentUserName", currentUserName)
                .UsingJobData("Email", email)
                .UsingJobData("Message", message)
                .UsingJobData("TemplateSubject", templateSubject)
                .Build();
            
            var trigger = TriggerBuilder.Create()
                .WithIdentity("emailsendtrigger", $"groupEmail{num}")
                .StartAt(DateBuilder.DateOf(
                    timeToStart.Hour,
                    timeToStart.Minute,
                    timeToStart.Second,
                    timeToStart.Day,
                    timeToStart.Month))
                .EndAt(DateBuilder.DateOf(
                    timeToStart.Hour,
                    timeToStart.Minute,
                    timeToStart.Second,
                    timeToStart.Day,
                    timeToStart.Month))
                .Build();

            await scheduler.ScheduleJob(job, trigger);

            await scheduler.Start();

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
