using BlankProject.Application.Interfaces;
using BlankProject.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlankProject.Infra.IoC
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection Register(IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IFooService, FooService>();

            return services;
        }
    }
}
