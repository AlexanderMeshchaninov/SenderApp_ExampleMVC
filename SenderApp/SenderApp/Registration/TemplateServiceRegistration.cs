using Microsoft.Extensions.DependencyInjection;
using SenderApp.Services.TemplateService;

namespace SenderApp.Registration
{
    public static class TemplateServiceRegistration
    {
        public static IServiceCollection RegisterTemplateService(this IServiceCollection service)
        {
            return service.AddTransient<ITemplateEngineService, TemplateEngineService>();
        }
    }
}