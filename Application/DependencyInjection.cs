using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Utils;
using FluentValidation;
using Amazon.Translate;
using Microsoft.Extensions.Configuration;
using Amazon.Runtime;
using Amazon;

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
                    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                });
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddSingleton<IAmazonTranslate>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                var accessKey = configuration["AWS:AccessKey"];
                var secretKey = configuration["AWS:SecretKey"];
                var region = configuration["AWS:Region"];

                var credentials = new BasicAWSCredentials(accessKey, secretKey);
                var regionEndpoint = RegionEndpoint.GetBySystemName(region);

                return new AmazonTranslateClient(credentials, regionEndpoint);
            });

            return services;
        }
    }
}
