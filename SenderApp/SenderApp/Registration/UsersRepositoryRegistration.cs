using Microsoft.Extensions.DependencyInjection;
using SenderApp.Data.UsersRepository;

namespace SenderApp.Registration
{
    public static class UsersRepositoryRegistration
    {
        public static IServiceCollection RegisterUsersRepository(this IServiceCollection service)
        {
            return service.AddTransient<IUserRepository, UsersRepository>();
        }
    }
}