using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Shared;
using static Domain.Entities.User.FailedLoginAttempts;

namespace Identity.Persistence
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> option) : DbContext(option)
    {

        public DbSet<UserAuthentication> Users { get; set; }
        public DbSet<FailedLoginAttempt> FailedLoginAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<UserAuthentication>(
                entity => 
                {
                    DbContextSingleton.ConfigureUserProperties<UserAuthentication>(entity);
                    entity.Property(e => e.Password).HasMaxLength(100);
                });
            modelBuilder.Entity<FailedLoginAttempt>(entity =>
            {
                entity.ToTable("failed_login_attempts");

                entity.HasKey(f => f.AttemptId);

                entity.Property(f => f.FailedAttempts)
                    .IsRequired();

                entity.Property(f => f.LastFailedAttemptTime)
                    .IsRequired();

                entity.Property(f => f.LockoutEndTime)
                    .IsRequired(false);

                entity.Property(f => f.MaxFailedLoginAttempts)
                    .HasDefaultValue(5)
                    .IsRequired();

                entity.HasOne(f => f.UserAuthentication)
                    .WithMany()
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}