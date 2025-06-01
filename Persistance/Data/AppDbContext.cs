using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<DeviceSetting> DeviceSettings { get; set; }
        public DbSet<DeviceAttribute> DeviceAttributes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configure Schedule
            modelBuilder.Entity<Schedule>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Name).IsRequired();

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Time).HasConversion<TimeSpan>();

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Days)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                );

            modelBuilder.Entity<Schedule>()
                .HasMany(s => s.DeviceSettings)
                .WithOne(ds => ds.Schedule)
                .HasForeignKey(ds => ds.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure DeviceSetting
            modelBuilder.Entity<DeviceSetting>()
                .HasKey(ds => ds.Id);

            modelBuilder.Entity<DeviceSetting>()
                .HasMany(ds => ds.Attributes)
                .WithOne(da => da.DeviceSetting)
                .HasForeignKey(da => da.DeviceSettingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure DeviceAttribute
            modelBuilder.Entity<DeviceAttribute>()
                .HasKey(da => da.Id);

            modelBuilder.Entity<DeviceAttribute>()
                .Property(da => da.Key).IsRequired();

            modelBuilder.Entity<DeviceAttribute>()
                .Property(da => da.Value).IsRequired();
        }

    }

}
