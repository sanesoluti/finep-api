namespace apifinep.DTOs
{
    public class DeviceDto
    {
        public int Id { get; set; }
        public string Serial { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal? MinFlow { get; set; }
        public decimal? MaxFlow { get; set; }
        public string? Logradouro { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? MinVolume { get; set; }
        public decimal? MaxVolume { get; set; }
        public string? Cep { get; set; }
        public string? Bairro { get; set; }
        public string? Numero { get; set; }
    }
}