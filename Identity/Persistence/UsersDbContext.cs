using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Identity
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> option) : DbContext(option)
    {

        public DbSet<UserImplementation> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<UserImplementation>(
                entity => DbContextSingleton.ConfigureUserProperties<UserImplementation>(entity));

        }
    }
}