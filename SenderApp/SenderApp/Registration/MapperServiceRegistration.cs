using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SenderApp.Services.AutoMapperService.MapperProfile;

namespace SenderApp.Registration
{
    public static class MapperServiceRegistration
    {
        public static IServiceCollection RegisterMapperService(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(mp => 
                mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            
            return services.AddSingleton(mapper);
        }
    }
}