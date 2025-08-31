using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apifinep.Models
{
    [Table("vw_device_statistics")]
    public class DeviceStatistics
    {
        [Column("device_id")]
        public int Id { get; set; }
        
        [Column("description")]
        public string Description { get; set; } = string.Empty;
        
        [Column("last_reading")]
        public DateTime? LastReading { get; set; }
        
        [Column("device_count")]
        public int DeviceCount { get; set; }

        [Column("total_readings")]
        public int TotalReadings { get; set; }
    }
}