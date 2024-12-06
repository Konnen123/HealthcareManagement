using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Identity
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> option) : DbContext(option)
    {

        public DbSet<UserImplementation> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<UserImplementation>(
               entity =>
               {
                   entity.ToTable("users");
                   entity.HasKey(e => e.Id);
                   entity.Property(e => e.Id)
                   .HasColumnType("uuid")
                   .HasDefaultValueSql("uuid_generate_v4()")
                   .ValueGeneratedOnAdd();

                   entity.Property(entity => entity.FirstName).IsRequired().HasMaxLength(50);
                   entity.Property(entity => entity.LastName).IsRequired().HasMaxLength(50);
                   entity.Property(entity => entity.Email).IsRequired().HasMaxLength(100);
                   entity.Property(entity => entity.Password).IsRequired().HasMaxLength(50);
                   entity.Property(entity => entity.DateOfBirth).IsRequired();
                   entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                   entity.Property(entity => entity.CreatedAt).IsRequired();
               });

        }
    }
}