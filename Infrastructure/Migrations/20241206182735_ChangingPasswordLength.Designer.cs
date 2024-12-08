﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241206182735_ChangingPasswordLength")]
    partial class ChangingPasswordLength
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

            modelBuilder.Entity("Domain.Entities.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime?>("CanceledAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CancellationReason")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uuid");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time without time zone");

                    b.Property<string>("Floor")
                        .HasColumnType("text");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uuid");

                    b.Property<string>("RoomNo")
                        .HasColumnType("text");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time without time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserNotes")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("appointments", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.AppointmentUpdateRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("AppointmentId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsProcessed")
                        .HasColumnType("boolean");

                    b.Property<DateOnly?>("NewDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly?>("NewStartTime")
                        .HasColumnType("time without time zone");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId");

                    b.ToTable("appointment_update_requests", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.DailyDoctorSchedule", b =>
                {
                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uuid");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("integer");

                    b.Property<Guid>("DailyDoctorScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<TimeOnly>("EndingTime")
                        .HasColumnType("time without time zone");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uuid");

                    b.Property<int>("SlotDurationMinutes")
                        .HasColumnType("integer");

                    b.Property<TimeOnly>("StartingTime")
                        .HasColumnType("time without time zone");

                    b.HasKey("DoctorId", "DayOfWeek");

                    b.HasIndex("DailyDoctorScheduleId")
                        .IsUnique();

                    b.HasIndex("LocationId");

                    b.ToTable("daily_doctor_schedules", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Location", b =>
                {
                    b.Property<Guid>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Floor")
                        .HasColumnType("integer");

                    b.Property<string>("Indications")
                        .HasColumnType("text");

                    b.Property<int>("RoomNo")
                        .HasColumnType("integer");

                    b.HasKey("LocationId");

                    b.HasIndex("RoomNo")
                        .IsUnique();

                    b.ToTable("locations", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.ScheduleIrregularity", b =>
                {
                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time without time zone");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<Guid>("ScheduleIrregularityId")
                        .HasColumnType("uuid");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time without time zone");

                    b.HasKey("DoctorId", "Date");

                    b.ToTable("schedule_irregularities", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User.User", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

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

            modelBuilder.Entity("Domain.Entities.Appointment", b =>
                {
                    b.HasOne("Domain.Entities.User.Doctor", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Domain.Entities.AppointmentUpdateRequest", b =>
                {
                    b.HasOne("Domain.Entities.Appointment", "Appointment")
                        .WithMany("UpdateRequests")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");
                });

            modelBuilder.Entity("Domain.Entities.DailyDoctorSchedule", b =>
                {
                    b.HasOne("Domain.Entities.User.Doctor", "Doctor")
                        .WithMany("DailySchedules")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Location", "Location")
                        .WithMany("DoctorSchedules")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Domain.Entities.ScheduleIrregularity", b =>
                {
                    b.HasOne("Domain.Entities.User.Doctor", "Doctor")
                        .WithMany("ScheduleIrregularities")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("Domain.Entities.User.Patient", b =>
                {
                    b.HasOne("Domain.Entities.User.User", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.User.Patient", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.User.Staff", b =>
                {
                    b.HasOne("Domain.Entities.User.User", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.User.Staff", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.User.Admin", b =>
                {
                    b.HasOne("Domain.Entities.User.Staff", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.User.Admin", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.User.Doctor", b =>
                {
                    b.HasOne("Domain.Entities.User.Staff", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.User.Doctor", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.User.SuperAdmin", b =>
                {
                    b.HasOne("Domain.Entities.User.Staff", null)
                        .WithOne()
                        .HasForeignKey("Domain.Entities.User.SuperAdmin", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Appointment", b =>
                {
                    b.Navigation("UpdateRequests");
                });

            modelBuilder.Entity("Domain.Entities.Location", b =>
                {
                    b.Navigation("DoctorSchedules");
                });

            modelBuilder.Entity("Domain.Entities.User.Doctor", b =>
                {
                    b.Navigation("DailySchedules");

                    b.Navigation("ScheduleIrregularities");
                });
#pragma warning restore 612, 618
        }
    }
}
