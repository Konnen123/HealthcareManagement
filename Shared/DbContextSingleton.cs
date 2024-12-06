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

        public static void ConfigureUserProperties<T>(EntityTypeBuilder<T> entity) where T: User
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedAt).IsRequired();
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
