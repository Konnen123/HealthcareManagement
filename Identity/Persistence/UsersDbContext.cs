using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Identity.Persistence
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> option) : DbContext(option)
    {

        public DbSet<UserAuthentication> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<UserAuthentication>(
                entity => 
                {
                    DbContextSingleton.ConfigureUserProperties<UserAuthentication>(entity);
                    entity.Property(e => e.Password).HasMaxLength(100);
                });
            
        }
    }
}