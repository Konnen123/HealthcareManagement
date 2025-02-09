﻿using Domain.Entities.Tokens;
using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> option) : DbContext(option)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Staff> Staffs { get; set; }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<FailedLoginAttempt> FailedLoginAttempts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }
        public DbSet<VerifyEmailToken> VerifyEmailTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<User>(
                entity =>
                {
                    entity.ToTable("users");
                    entity.HasKey(e => e.UserId);
                    entity.Property(e => e.UserId)
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()")
                        .ValueGeneratedOnAdd();

                    entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                    entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                    entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                    entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
                    entity.Property(e => e.DateOfBirth).IsRequired();
                    entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                    entity.Property(e => e.CreatedAt).IsRequired();
                });

            modelBuilder.Entity<Patient>(
                 entity =>
                 {
                     entity.ToTable("patients");
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
            
            modelBuilder.Entity<RefreshToken>()
                .ToTable("RefreshTokens")
                .Property(t => t.RefreshTokenId)
                .HasDefaultValueSql("uuid_generate_v4()");
            
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(sm => sm.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ResetPasswordToken>(entity =>
            {
                entity.ToTable("reset_password_tokens");
                
                entity.HasKey(t => t.ResetPasswordTokenId);
                
                entity.Property(t => t.ResetPasswordTokenId)
                    .HasDefaultValueSql("uuid_generate_v4()");
                
                entity.Property(t => t.Token)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(t => t.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .HasConversion(
                          v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                          v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                      );

                entity.Property(t => t.ExpiresAt)
                      .HasConversion(
                          v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                          v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                      );

                entity.HasOne(t => t.UserAuthentication)
                    .WithOne(t => t.ResetPasswordToken)
                    .HasForeignKey<ResetPasswordToken>(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            
            modelBuilder.Entity<VerifyEmailToken>(entity =>
            {
                entity.ToTable("verify_email_tokens");
                
                entity.HasKey(t => t.VerifyEmailTokenId);
                
                entity.Property(t => t.VerifyEmailTokenId)
                    .HasDefaultValueSql("uuid_generate_v4()");
                
                entity.Property(t => t.Token)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(t => t.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(t => t.ExpiresAt)
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.HasOne(t => t.UserAuthentication)
                    .WithOne(t => t.VerifyEmailToken)
                    .HasForeignKey<VerifyEmailToken>(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}