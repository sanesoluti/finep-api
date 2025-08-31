using System.ComponentModel.DataAnnotations;

namespace apifinep.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username é obrigatório")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password é obrigatório")]
        public string Password { get; set; } = string.Empty;
    }
    
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username deve ter entre 3 e 50 caracteres")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "FullName é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "FullName deve ter pelo menos 2 caracteres")]
        public string FullName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password é obrigatório")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password deve ter pelo menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;
        
        [StringLength(20, ErrorMessage = "Role deve ter no máximo 20 caracteres")]
        public string Role { get; set; } = "User";
    }
    
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
    
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}