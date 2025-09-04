namespace apifinep.DTOs
{
    public class DeviceStatisticsDto
    {
        public int DeviceId { get; set; }
        public string Serial { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public decimal Flow { get; set; }
        public decimal Volume { get; set; }
        public decimal Pressure { get; set; }
        public decimal Vibration { get; set; }
        public string Flow_Unit { get; set; } = string.Empty;
        public string Volume_Unit { get; set; } = string.Empty;
        public string Pressure_Unit { get; set; } = string.Empty;
        public string Vibration_Unit { get; set; } = string.Empty;
    }
    
    public class DeviceStatisticsFilterDto
    {
        public string? Description { get; set; }
        public DateTime? LastReadingInicio { get; set; }
        public DateTime? LastReadingFim { get; set; }
    }
}