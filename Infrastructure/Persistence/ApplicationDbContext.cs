using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option, IConfiguration configuration) : base(option)
        {
            this.configuration = configuration;
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Pacient> Pacients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = configuration["ConnectionStrings:Host"];
            var port = configuration["ConnectionStrings:Port"];
            var username = configuration["ConnectionStrings:Username"];
            var password = configuration["ConnectionStrings:Password"];
            var database = configuration["ConnectionStrings:Database"];

            var connectionString = $"Host={host};Port={port};Username={username};Password={password};Database={database}";
            optionsBuilder.UseNpgsql(connectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<User>(
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
                }
            );
            modelBuilder.Entity<Pacient>(
                 entity =>
                 {
                     entity.ToTable("pacients");
                     entity.HasBaseType<User>();
                 });
            modelBuilder.Entity<Staff>(
                entity =>
                {
                    entity.ToTable("staffs");
                    entity.HasBaseType<User>();
                    entity.Property(entity => entity.MedicalRank).IsRequired().HasMaxLength(50);                    
                });
            modelBuilder.Entity<Doctor>(
                entity =>
                {
                    entity.ToTable("doctors");
                    entity.HasBaseType<Staff>();
                });
            modelBuilder.Entity<Admin>(
                entity =>
                {
                    entity.ToTable("admins");
                    entity.HasBaseType<Staff>();
                });
            modelBuilder.Entity<SuperAdmin>(
                entity => 
                {
                    entity.ToTable("super_admins");
                    entity.HasBaseType<Staff>();
                });

            modelBuilder.Entity<Appointment>(
                entity =>
                {
                    entity.ToTable("appointments");
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .ValueGeneratedOnAdd();

                    entity.Property(entity => entity.Date).IsRequired();
                    entity.Property(entity => entity.StartTime).IsRequired();
                    entity.Property(entity => entity.EndTime).IsRequired();
                });

            

        }
    }
}
