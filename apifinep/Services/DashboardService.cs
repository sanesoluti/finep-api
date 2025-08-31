using Microsoft.EntityFrameworkCore;
using apifinep.Data;
using apifinep.DTOs;
using apifinep.Models;

namespace apifinep.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync(DashboardFilterDto filter);
    }

    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync(DashboardFilterDto filter)
        {
            var query = _context.DeviceStatistics.AsQueryable();

            // Aplicar filtros se fornecidos
            if (!string.IsNullOrEmpty(filter.Description))
            {
                query = query.Where(d => d.Description.Contains(filter.Description));
            }

            if (filter.LastReadingInicio.HasValue)
            {
                query = query.Where(d => d.LastReading >= filter.LastReadingInicio.Value);
            }

            if (filter.LastReadingFim.HasValue)
            {
                query = query.Where(d => d.LastReading <= filter.LastReadingFim.Value);
            }

            var data = await query.ToListAsync();

            return new DashboardSummaryDto
            {
                TotalRegistros = data.Sum(d => d.TotalReadings), //data.Count,
                TotalDispositivos = data.Sum(d => d.DeviceCount),
                UltimoUpdate = data.Max(d => d.LastReading)
            };
        }
    }
}