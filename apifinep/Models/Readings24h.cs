using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apifinep.Models
{
    [Table("vw_readings_24h")]
    public class Readings24h
    {
        [Column("device_id")]
        public int DeviceId { get; set; }
        
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
        
        [Column("flow")]
        public decimal Flow { get; set; }
        
        [Column("volume")]
        public decimal Volume { get; set; }
        
        [Column("unit")]
        public string Unit { get; set; } = string.Empty;
    }
}