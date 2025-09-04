using Microsoft.EntityFrameworkCore;
using apifinep.Data;
using apifinep.DTOs;
using apifinep.Models;

namespace apifinep.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync(DashboardFilterDto filter);
        Task<List<DeviceStatisticsDto>> GetDeviceStatisticsAsync(DeviceStatisticsFilterDto filter);
        Task<List<Readings24hDto>> GetReadings24hAsync(Readings24hFilterDto filter);
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

        public async Task<List<DeviceStatisticsDto>> GetDeviceStatisticsAsync(DeviceStatisticsFilterDto filter)
        {
            var query = _context.LatestReadings.AsQueryable();

            // Aplicar filtros se fornecidos
            if (!string.IsNullOrEmpty(filter.Description))
            {
                query = query.Where(d => d.Description.Contains(filter.Description));
            }

            if (filter.LastReadingInicio.HasValue)
            {
                query = query.Where(d => d.Timestamp >= filter.LastReadingInicio.Value);
            }

            if (filter.LastReadingFim.HasValue)
            {
                query = query.Where(d => d.Timestamp <= filter.LastReadingFim.Value);
            }

            var data = await query.ToListAsync();

            return data.Select(d => new DeviceStatisticsDto
            {
                DeviceId = d.DeviceId,
                Serial = d.Serial,
                Description = d.Description,
                Timestamp = d.Timestamp,
                Flow = d.Flow,
                Volume = d.Volume,
                Pressure = d.Pressure,
                Vibration = d.Vibration,
                Flow_Unit = d.Flow_Unit,
                Volume_Unit = d.Volume_Unit,
                Pressure_Unit = d.Pressure_Unit,
                Vibration_Unit = d.Vibration_Unit
            }).ToList();
        }

        public async Task<List<Readings24hDto>> GetReadings24hAsync(Readings24hFilterDto filter)
        {
            var query = _context.Readings24h
                .Where(r => r.DeviceId == filter.DeviceId)
                .AsQueryable();

            var data = await query.ToListAsync();

            return data.Select(d => new Readings24hDto
            {
                Timestamp = d.Timestamp,
                Value = filter.Reading.ToLower() == "flow" ? d.Flow : d.Volume,
                Unit = d.Unit
            }).ToList();
        }
    }
}