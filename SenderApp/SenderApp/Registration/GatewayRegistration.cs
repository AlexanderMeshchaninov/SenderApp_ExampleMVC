using Core.Abstractions.Gateway;
using Microsoft.Extensions.DependencyInjection;
using SenderApp.CommonLogic.Gateway;

namespace SenderApp.Registration
{
    public static class GatewayRegistration
    {
        public static IServiceCollection RegisterGateway(this IServiceCollection service)
        {
            return service.AddTransient<IGateway, Gateway>();
        }
    }
}