using Core.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using SenderApp.Services.QuartzService.Schedulers;

namespace SenderApp.Registration
{
    public static class SendEmailSchedulerRegistration
    {
        public static IServiceCollection RegisterSendEmailScheduler(this IServiceCollection service)
        {
            return service
                .AddTransient<ISchedulerFactory, StdSchedulerFactory>()
                .AddTransient<ISendScheduler, SendEmailScheduler>();
        }
    }
}