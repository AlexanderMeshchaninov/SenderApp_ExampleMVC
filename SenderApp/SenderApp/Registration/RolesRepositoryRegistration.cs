using Microsoft.Extensions.DependencyInjection;
using SenderApp.Data.RolesRepository;

namespace SenderApp.Registration
{
    public static class RolesRepositoryRegistration
    {
        public static IServiceCollection RegisterRolesRepository(this IServiceCollection service)
        {
            return service.AddTransient<IRolesRepository, RolesRepository>();
        }
    }
}