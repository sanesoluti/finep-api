using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}