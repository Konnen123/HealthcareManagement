using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared
{
    public class DbContextSingleton
    {
        private DbContextSingleton()
        {

        }

        public static void AddDbContext<T>(IServiceCollection services, IConfiguration configuration, string connectionPrefix) where T : DbContext
        {
            string connectionString = GetConnectionString(configuration, connectionPrefix);

            services.AddDbContext<T>(options =>
                options.UseNpgsql(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(T).Assembly.FullName)));
        }

        private static string GetConnectionString(IConfiguration configuration, string connectionPrefix)
        {
            var host = configuration[$"{connectionPrefix}Host"];
            var port = configuration[$"{connectionPrefix}Port"];
            var username = configuration[$"{connectionPrefix}Username"];
            var password = configuration[$"{connectionPrefix}Password"];
            var database = configuration[$"{connectionPrefix}Database"];

            return $"Host={host};Port={port};Username={username};Password={password};Database={database};Include Error Detail=true";
        }
    }
}
