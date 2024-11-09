using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
            IConfiguration configuration)
        {
            string connectionString = GetConnectionString(configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IRescheduledAppointmentsRepository, RescheduledAppointmentsRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            return services;
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            var host = configuration["ConnectionStrings:Host"];
            var port = configuration["ConnectionStrings:Port"];
            var username = configuration["ConnectionStrings:Username"];
            var password = configuration["ConnectionStrings:Password"];
            var database = configuration["ConnectionStrings:Database"];

            return $"Host={host};Port={port};Username={username};Password={password};Database={database};Include Error Detail=true";
        }
    }
}
