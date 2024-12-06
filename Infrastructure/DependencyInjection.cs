using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
            IConfiguration configuration, string connectionStringPrefix)
        {
            DbContextSingleton.AddDbContext<ApplicationDbContext>(services, configuration, connectionStringPrefix);

            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IRescheduledAppointmentsRepository, RescheduledAppointmentsRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ISchedulesRepository, SchedulesRepository>();
            return services;
        }
    }
}
