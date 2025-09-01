using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using apifinep.DTOs;
using apifinep.Services;

namespace apifinep.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Obtém resumo geral com estatísticas de dispositivos
        /// </summary>
        /// <param name="description">Filtro por descrição (opcional)</param>
        /// <param name="lastReadingInicio">Data início para filtro de última leitura (opcional)</param>
        /// <param name="lastReadingFim">Data fim para filtro de última leitura (opcional)</param>
        /// <returns>Resumo com total de registros, dispositivos e último update</returns>
        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryDto>> GetSummary(
            [FromQuery] string? description = null,
            [FromQuery] DateTime? lastReadingInicio = null,
            [FromQuery] DateTime? lastReadingFim = null)
        {
            try
            {
                var filter = new DashboardFilterDto
                {
                    Description = description,
                    LastReadingInicio = lastReadingInicio,
                    LastReadingFim = lastReadingFim
                };

                var summary = await _dashboardService.GetSummaryAsync(filter);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém estatísticas detalhadas dos dispositivos
        /// </summary>
        /// <param name="description">Filtro por descrição (opcional)</param>
        /// <param name="lastReadingInicio">Data início para filtro de última leitura (opcional)</param>
        /// <param name="lastReadingFim">Data fim para filtro de última leitura (opcional)</param>
        /// <returns>Lista com estatísticas detalhadas dos dispositivos</returns>
        [HttpGet("devices/statistics")]
        public async Task<ActionResult<List<DeviceStatisticsDto>>> GetDeviceStatistics(
            [FromQuery] string? description = null,
            [FromQuery] DateTime? lastReadingInicio = null,
            [FromQuery] DateTime? lastReadingFim = null)
        {
            try
            {
                var filter = new DeviceStatisticsFilterDto
                {
                    Description = description,
                    LastReadingInicio = lastReadingInicio,
                    LastReadingFim = lastReadingFim
                };

                var statistics = await _dashboardService.GetDeviceStatisticsAsync(filter);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém leituras das últimas 24 horas de um dispositivo específico
        /// </summary>
        /// <param name="deviceId">ID do dispositivo (obrigatório)</param>
        /// <param name="reading">Tipo de leitura: 'flow' ou 'volume' (obrigatório)</param>
        /// <returns>Lista com timestamp, valor e unidade das leituras</returns>
        [HttpGet("devices/readings-24h")]
        public async Task<ActionResult<List<Readings24hDto>>> GetDeviceReadings24h(
            [FromQuery] int deviceId,
            [FromQuery] string reading)
        {
            try
            {
                // Validar parâmetros obrigatórios
                if (deviceId <= 0)
                {
                    return BadRequest(new { message = "Device ID deve ser maior que zero" });
                }

                if (string.IsNullOrEmpty(reading) || (reading.ToLower() != "flow" && reading.ToLower() != "volume"))
                {
                    return BadRequest(new { message = "Reading deve ser 'flow' ou 'volume'" });
                }

                var filter = new Readings24hFilterDto
                {
                    DeviceId = deviceId,
                    Reading = reading
                };

                var readings = await _dashboardService.GetReadings24hAsync(filter);
                return Ok(readings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }
    }
}