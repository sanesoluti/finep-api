namespace apifinep.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalRegistros { get; set; }
        public int TotalDispositivos { get; set; }
        public DateTime? UltimoUpdate { get; set; }
    }
    
    public class DashboardFilterDto
    {
        public string? Description { get; set; }
        public DateTime? LastReadingInicio { get; set; }
        public DateTime? LastReadingFim { get; set; }
    }
}