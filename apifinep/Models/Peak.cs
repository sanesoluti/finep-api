using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apifinep.Models
{
    [Table("peaks")]
    public class Peak
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("peaks_devices_id")]
        public int PeaksDevicesId { get; set; }

        [Column("sequence")]
        public int Sequence { get; set; }

        [Column("type")]
        public string Type { get; set; } = string.Empty;

        [Column("value")]
        public decimal Value { get; set; }

        [Column("address")]
        public int Address { get; set; }

        [Column("factory")]
        public int Factory { get; set; }

        // Navigation property
        public PeakDevice PeakDevice { get; set; } = null!;
    }
}