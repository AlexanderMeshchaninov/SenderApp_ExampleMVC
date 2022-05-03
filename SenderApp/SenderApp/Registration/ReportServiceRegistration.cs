using Microsoft.Extensions.DependencyInjection;
using SenderApp.Services.ReportService;

namespace SenderApp.Registration
{
    public static class ReportServiceRegistration
    {
        public static IServiceCollection RegistryReportService(this IServiceCollection service)
        {
            return service.AddSingleton<IReportService, ReportService>();
        }
    }
}