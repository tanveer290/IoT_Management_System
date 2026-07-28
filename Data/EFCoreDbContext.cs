using System.Configuration;
using IOTDeviceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace IOTDeviceManagementSystem.Data
{
    public class EFCoreDbContext : DbContext
    {
        public EFCoreDbContext() 
        {

        }

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (optionsBuilder.IsConfigured) return;

            
            var connectionSetting = ConfigurationManager.ConnectionStrings["ConnectionString"];

            
            if (connectionSetting != null && !string.IsNullOrWhiteSpace(connectionSetting.ConnectionString))
            {
                
                optionsBuilder.UseSqlServer(connectionSetting.ConnectionString);
            }
            else
            {
                return;
                //optionsBuilder.UseSqlServer(@"Server=tanveer\SQLEXPRESS;Database=IOT_Device_Management_DB;Integrated Security=True;TrustServerCertificate=True;");
            }
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Telemetry> Telemetries { get; set; }

}
}

