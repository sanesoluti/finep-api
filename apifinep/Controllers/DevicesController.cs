using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using apifinep.Data;
using apifinep.DTOs;
using apifinep.Models;

namespace apifinep.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DevicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém informações específicas de um dispositivo
        /// </summary>
        /// <param name="id">ID do dispositivo (obrigatório)</param>
        /// <returns>Informações completas do dispositivo</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceDto>> GetDevice(int id)
        {
            try
            {
                // Validar parâmetro obrigatório
                if (id <= 0)
                {
                    return BadRequest(new { message = "Device ID deve ser maior que zero" });
                }

                var device = await _context.Devices
                    .Where(d => d.Id == id)
                    .FirstOrDefaultAsync();

                if (device == null)
                {
                    return NotFound(new { message = $"Dispositivo com ID {id} não encontrado" });
                }

                var deviceDto = new DeviceDto
            {
                Id = device.Id,
                Serial = device.Serial,
                Description = device.Description,
                Status = device.Status,
                CreatedAt = device.CreatedAt,
                UpdatedAt = device.UpdatedAt,
                MinFlow = device.MinFlow,
                MaxFlow = device.MaxFlow,
                Logradouro = device.Logradouro,
                Cidade = device.Cidade,
                Estado = device.Estado,
                Latitude = device.Latitude,
                Longitude = device.Longitude,
                MinVolume = device.MinVolume,
                MaxVolume = device.MaxVolume,
                Cep = device.Cep,
                Bairro = device.Bairro,
                Numero = device.Numero
            };

                return Ok(deviceDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }
        
        /// <summary>
        /// Obter peaks dos dispositivos/equipamentos de medição
        /// </summary>
        /// <param name="serial">Serial do dispositivo (obrigatório)</param>
        /// <param name="factory">Factory (0 ou 1) (obrigatório)</param>
        /// <param name="type">Tipo (DNS ou UPS) (obrigatório)</param>
        /// <returns>Lista de peaks com sequence e value</returns>
        [HttpGet("peaks")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PeakDto>>> GetPeaks(
            [FromQuery] string serial,
            [FromQuery] int factory,
            [FromQuery] string type)
        {
            // Validate required parameters
            if (string.IsNullOrEmpty(serial))
            {
                return BadRequest("Serial parameter is required");
            }

            if (factory != 0 && factory != 1)
            {
                return BadRequest("Factory parameter must be 0 or 1");
            }

            if (string.IsNullOrEmpty(type) || (type != "DNS" && type != "UPS"))
            {
                return BadRequest("Type parameter must be 'DNS' or 'UPS'");
            }

            try
            {
                var peaks = await _context.Peaks
                    .Include(p => p.PeakDevice)
                    .Where(p => p.PeakDevice.Serial == serial && p.Factory == factory && p.Type == type)
                    .Select(p => new PeakDto
                    {
                        Sequence = p.Sequence,
                        Value = p.Value
                    })
                    .ToListAsync();

                return Ok(peaks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        

     }
}