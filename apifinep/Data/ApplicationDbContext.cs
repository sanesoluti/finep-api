using Microsoft.EntityFrameworkCore;
using apifinep.Models;

namespace apifinep.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DeviceStatistics> DeviceStatistics { get; set; }
        public DbSet<LatestReading> LatestReadings { get; set; }
        public DbSet<Readings24h> Readings24h { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configurar as views
            modelBuilder.Entity<DeviceStatistics>()
                .ToView("vw_device_statistics")
                .HasNoKey();
                
            modelBuilder.Entity<LatestReading>()
                .ToView("vw_latest_readings")
                .HasNoKey();
                
            modelBuilder.Entity<Readings24h>()
                .ToView("vw_readings_24h")
                .HasNoKey();
        }
    }
}