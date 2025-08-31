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
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configurar a view
            modelBuilder.Entity<DeviceStatistics>()
                .ToView("vw_device_statistics")
                .HasNoKey();
        }
    }
}