using Domain.Entities;
using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : DbContext(option)
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentUpdateRequest> AppointmentUpdateRequests { get; set; }
        public DbSet<DailyDoctorSchedule> DailyDoctorSchedules { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ScheduleIrregularity> ScheduleIrregularities { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    
                    entity.Property(entity => entity.PatientId).IsRequired();

                    entity.Property(entity => entity.DoctorId).IsRequired();
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
