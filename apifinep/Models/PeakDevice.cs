using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apifinep.Models
{
    [Table("peaks_devices")]
    public class PeakDevice
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("serial")]
        public string Serial { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Peak> Peaks { get; set; } = new List<Peak>();
    }
}