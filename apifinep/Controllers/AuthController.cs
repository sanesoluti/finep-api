using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using apifinep.DTOs;
using apifinep.Services;
using System.Security.Claims;

namespace apifinep.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Realiza login do usuário
        /// </summary>
        /// <param name="loginDto">Dados de login (username e password)</param>
        /// <returns>Token JWT e informações do usuário</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.LoginAsync(loginDto);
                
                if (result == null)
                {
                    return Unauthorized(new { message = "Credenciais inválidas" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        /// <param name="registerDto">Dados para registro do usuário</param>
        /// <returns>Informações do usuário criado</returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.RegisterAsync(registerDto);
                
                if (result == null)
                {
                    return BadRequest(new { message = "Username ou email já existem" });
                }

                return CreatedAtAction(nameof(GetProfile), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém o perfil do usuário autenticado
        /// </summary>
        /// <returns>Informações do usuário</returns>
        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Token inválido" });
                }

                var user = await _authService.GetUserByIdAsync(userId);
                
                if (user == null)
                {
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Verifica se o token JWT é válido
        /// </summary>
        /// <returns>Status da validação do token</returns>
        [HttpGet("validate")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                
                return Ok(new 
                { 
                    valid = true, 
                    username = username,
                    role = role,
                    message = "Token válido" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }
    }
}