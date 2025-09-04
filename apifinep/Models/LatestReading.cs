using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apifinep.Models
{
    [Table("vw_latest_readings")]
    public class LatestReading
    {
        [Column("device_id")]
        public int DeviceId { get; set; }
        
        [Column("serial")]
        public string Serial { get; set; } = string.Empty;
        
        [Column("description")]
        public string Description { get; set; } = string.Empty;
        
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
        
        [Column("flow")]
        public decimal Flow { get; set; }
        
        [Column("volume")]
        public decimal Volume { get; set; }

        [Column("pressure")]
        public decimal Pressure { get; set; }

        [Column("vibration")]
        public decimal Vibration { get; set; }

        [Column("flow_unit")]
        public string Flow_Unit { get; set; } = string.Empty;
        
        [Column("volume_unit")]
        public string Volume_Unit { get; set; } = string.Empty;    

        [Column("pressure_unit")]
        public string Pressure_Unit { get; set; } = string.Empty;

        [Column("vibration_unit")]
        public string Vibration_Unit { get; set; } = string.Empty;
    }
}