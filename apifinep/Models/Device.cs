using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apifinep.Models
{
    [Table("devices")]
    public class Device
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("serial")]
        public string Serial { get; set; } = string.Empty;
        
        [Column("description")]
        public string? Description { get; set; }

        [Column("latitude")]
        public decimal? Latitude { get; set; }
        
        [Column("longitude")]
        public decimal? Longitude { get; set; }

        [Column("min_flow")]
        public decimal? MinFlow { get; set; }
        
        [Column("max_flow")]
        public decimal? MaxFlow { get; set; }

        [Column("min_volume")]
        public decimal? MinVolume { get; set; }
        
        [Column("max_volume")]
        public decimal? MaxVolume { get; set; }
        
        [Column("cep")]
        public string? Cep { get; set; }
        
        [Column("logradouro")]
        public string? Logradouro { get; set; }

        [Column("bairro")]
        public string? Bairro { get; set; }
        
        [Column("cidade")]
        public string? Cidade { get; set; }
        
        [Column("estado")]
        public string? Estado { get; set; }
        
        [Column("numero")]
        public string? Numero { get; set; }

        [Column("status")]
        public string Status { get; set; } = "Active";
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}