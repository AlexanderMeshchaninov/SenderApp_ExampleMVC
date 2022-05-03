using System;

namespace SenderApp.Services.QuartzService.Jobs
{
    public sealed class JobSchedule
    {
        public Type JobType { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string TemplateSubject { get; set; }
        public DateTime TimeToStart { get; set; }

        public JobSchedule()
        { 
        }
        public JobSchedule(
            Type jobType, 
            string email, 
            string message, 
            string templateSubject, 
            DateTime timeToStart)
        {
            JobType = jobType;
            Email = email;
            Message = message;
            TemplateSubject = templateSubject;
            TimeToStart = timeToStart;
        }
    }
}