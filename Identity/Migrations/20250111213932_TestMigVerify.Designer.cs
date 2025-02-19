﻿// <auto-generated />
using System;
using Identity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Identity.Migrations
{
    [DbContext(typeof(UsersDbContext))]
    [Migration("20250111213932_TestMigVerify")]
    partial class TestMigVerify
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Tokens.RefreshToken", b =>
                {
                    b.Property<Guid>("RefreshTokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("DeviceInfo")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("IpAddress")
                        .HasColumnType("text");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("RefreshTokenId");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Tokens.ResetPasswordToken", b =>
                {
                    b.Property<Guid>("ResetPasswordTokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("ResetPasswordTokenId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("reset_password_tokens", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Tokens.VerifyEmailToken", b =>
                {
                    b.Property<Guid>("VerifyEmailTokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("VerifyEmailTokenId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("verify_email_tokens", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User.FailedLoginAttempt", b =>
                {
                    b.Property<Guid>("AttemptId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("FailedAttempts")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LastFailedAttemptTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LockoutEndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MaxFailedLoginAttempts")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(5);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("AttemptId");

                    b.HasIndex("UserId");

                    b.ToTable("failed_login_attempts", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateOnly>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("HasVerifiedEmail")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("UserId");

                    b.ToTable("users", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Domain.Entities.User.Patient", b =>
                {
                    b.HasBaseType("Domain.Entities.User.User");

                    b.ToTable("patients", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User.Staff", b =>
                {
                    b.HasBaseType("Domain.Entities.User.User");

                    b.Property<string>("MedicalRank")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.ToTable("staffs", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User.Admin", b =>
                {
                    b.HasBaseType("Domain.Entities.User.Staff");

                    b.ToTable("admins", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User.Doctor", b =>
                {
                    b.HasBaseType("Domain.Entities.User.Staff");

                    b.ToTable("doctors", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User.SuperAdmin", b =>
                {
                    b.HasBaseType("Domain.Entities.User.Staff");

                    b.ToTable("super_admins", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Tokens.RefreshToken", b =>
                {
                    b.HasOne("Domain.Entities.User.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Tokens.ResetPasswordToken", b =>
                {
                    b.HasOne("Domain.Entities.User.User", "UserAuthentication")
                        .WithOne("ResetPasswordToken")
                        .HasForeignKey("Domain.Entities.Tokens.ResetPasswordToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserAuthentication");
                });

            modelBuilder.Entity("Domain.Entities.Tokens.VerifyEmailToken", b =>
                {
                    b.HasOne("Domain.Entities.User.User", "UserAuthentication")
                        .WithOne("VerifyEmailToken")
                        .HasForeignKey("Domain.Entities.Tokens.VerifyEmailToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserAuthentication");
                });

            modelBuilder.Entity("Domain.Entities.User.FailedLoginAttempt", b =>
                {
                    b.HasOne("Domain.Entities.User.User", "UserAuthentication")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserAuthentication");
                });

            modelBuilder.Entity("Domain.Entities.User.Patient", b =>
                {
                    b.HasOne("Domain.Entities.User.User", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.User.Patient", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.User.Staff", b =>
                {
                    b.HasOne("Domain.Entities.User.User", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.User.Staff", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.User.Admin", b =>
                {
                    b.HasOne("Domain.Entities.User.Staff", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.User.Admin", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.User.Doctor", b =>
                {
                    b.HasOne("Domain.Entities.User.Staff", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.User.Doctor", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.User.SuperAdmin", b =>
                {
                    b.HasOne("Domain.Entities.User.Staff", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.User.SuperAdmin", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.User.User", b =>
                {
                    b.Navigation("RefreshTokens");

                    b.Navigation("ResetPasswordToken")
                        .IsRequired();

                    b.Navigation("VerifyEmailToken")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
