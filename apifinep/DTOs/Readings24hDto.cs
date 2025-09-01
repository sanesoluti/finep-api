using System.ComponentModel.DataAnnotations;

namespace apifinep.DTOs
{
    public class Readings24hDto
    {
        public DateTime Timestamp { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
    
    public class Readings24hFilterDto
    {
        [Required(ErrorMessage = "Device ID é obrigatório")]
        public int DeviceId { get; set; }
        
        [Required(ErrorMessage = "Tipo de leitura é obrigatório")]
        [RegularExpression("^(flow|volume)$", ErrorMessage = "Reading deve ser 'flow' ou 'volume'")]
        public string Reading { get; set; } = string.Empty;
    }
}