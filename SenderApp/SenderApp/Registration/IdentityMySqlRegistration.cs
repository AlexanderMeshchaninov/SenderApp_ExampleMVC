using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SenderApp.Data.Identity;

namespace SenderApp.Registration
{
    public static class IdentityMySqlRegistration
    {
        public static IServiceCollection RegisterMySqlIdentityProvider(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<SenderIdentityDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("ConnectionString"),
                    new MySqlServerVersion(new Version(8,0,29))));
        }
    }
}