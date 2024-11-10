using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{ 
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : DbContext(option)
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<AppointmentUpdateRequest> AppointmentUpdateRequests { get; set; }
        public DbSet<DailyDoctorSchedule> DailyDoctorSchedules { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ScheduleIrregularity> ScheduleIrregularities { get; set; }
        
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
                    entity.HasMany(d => d.DailySchedules)
                        .WithOne(ds => ds.Doctor)
                        .HasForeignKey(ds => ds.DoctorId)
                        .OnDelete(DeleteBehavior.Cascade);
                    
                    entity.HasMany(d => d.ScheduleIrregularities)
                        .WithOne(si => si.Doctor)
                        .HasForeignKey(si => si.DoctorId)
                        .OnDelete(DeleteBehavior.Cascade);
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
                    
                    entity.HasMany(a => a.UpdateRequests)
                        .WithOne(ur => ur.Appointment)
                        .HasForeignKey(ur => ur.AppointmentId)
                        .OnDelete(DeleteBehavior.Cascade);
                    
                    entity.Property(entity => entity.Date).IsRequired();
                    entity.Property(entity => entity.StartTime).IsRequired();
                    entity.Property(entity => entity.EndTime).IsRequired();
                    
                    entity.HasOne(e => e.Patient)
                        .WithMany()
                        .HasForeignKey(e => e.PatientId)
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasOne(e => e.Doctor)
                        .WithMany()
                        .HasForeignKey(e => e.DoctorId)
                        .OnDelete(DeleteBehavior.Cascade);
                });
            modelBuilder.Entity<AppointmentUpdateRequest>(entity =>
            {
                entity.ToTable("appointment_update_requests");
                entity.Property(e => e.Id)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<DailyDoctorSchedule>(entity =>
            {
                entity.ToTable("daily_doctor_schedules");
              
                entity.HasKey(ds => new { ds.DoctorId, ds.DayOfWeek });

               
                entity.Property(ds => ds.DailyDoctorScheduleId)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .IsRequired();
                
                entity.HasIndex(ds => ds.DailyDoctorScheduleId).IsUnique();

        
                entity.HasOne(ds => ds.Location)
                      .WithMany(l => l.DoctorSchedules)
                      .HasForeignKey(ds => ds.LocationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });



            modelBuilder.Entity<ScheduleIrregularity>().ToTable("schedule_irregularities");
            modelBuilder.Entity<ScheduleIrregularity>()
                .HasKey(si => new { si.DoctorId, si.Date });


            modelBuilder.Entity<Location>().ToTable("locations");
            modelBuilder.Entity<Location>()
                .HasIndex(l => l.RoomNo).IsUnique();
        }
    }
}
