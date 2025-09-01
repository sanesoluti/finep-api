namespace apifinep.DTOs
{
    public class PeakDto
    {
        public int Sequence { get; set; }
        public decimal Value { get; set; }
    }
    
    public class PeakFilterDto
    {
        public string Serial { get; set; } = string.Empty;
        public int Factory { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}