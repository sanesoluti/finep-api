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
    }
}