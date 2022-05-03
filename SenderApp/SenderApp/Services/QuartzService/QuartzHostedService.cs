using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using SenderApp.Services.QuartzService.Jobs;

namespace SenderApp.Services.QuartzService
{
    public sealed class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;
        private CancellationTokenSource _cts;
        public IScheduler Scheduler { get; set; }

        public QuartzHostedService
        (
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<JobSchedule> jobSchedules)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobSchedules = jobSchedules;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Scheduler = await _schedulerFactory.GetScheduler(_cts.Token);
            Scheduler.JobFactory = _jobFactory;

            foreach (var jobSchedule in _jobSchedules)
            {
                var job = CreateJobDetail(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);

                await Scheduler.ScheduleJob(job, trigger, _cts.Token);
            }

            await Scheduler.Start(_cts.Token);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(_cts.Token)!;
        }
        
        private static IJobDetail CreateJobDetail(JobSchedule jobSchedule)
        {
            var jobType = jobSchedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity($"{jobType.FullName}")
                .WithDescription($"{jobType.Name}")
                .UsingJobData("Email", jobSchedule.Email)
                .UsingJobData("Message", jobSchedule.Message)
                .UsingJobData("TemplateSubject", jobSchedule.TemplateSubject)
                .Build();
        }
        
        private static ITrigger CreateTrigger(JobSchedule jobSchedule)
        {
            return TriggerBuilder.Create()
                .WithIdentity($"{jobSchedule.JobType}.trigger")
                .StartAt(DateBuilder.DateOf(
                    jobSchedule.TimeToStart.Hour,
                    jobSchedule.TimeToStart.Minute,
                    jobSchedule.TimeToStart.Second,
                    jobSchedule.TimeToStart.Day,
                    jobSchedule.TimeToStart.Month))
                .WithPriority(1)
                .Build();
        }
    }
}