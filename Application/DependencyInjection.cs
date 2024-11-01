using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Application.Utils;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddMediatR(
                cfg =>
                {
                    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                });

            return services;
        }
    }
}
