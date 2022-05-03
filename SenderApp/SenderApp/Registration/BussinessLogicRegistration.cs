using Microsoft.Extensions.DependencyInjection;
using SenderApp.CommonLogic.BussinessLogic;

namespace SenderApp.Registration
{
    public static class BussinessLogicRegistration
    {
        public static IServiceCollection RegisterBussinessLayer(this IServiceCollection service)
        {
            return service.AddTransient<IBussinessLayer, BussinessLogic>();
        }
    }
}