using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apifinep.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("username")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [Column("email")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [Column("full_name")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;
        
        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
        
        [Column("role")]
        [MaxLength(20)]
        public string Role { get; set; } = "User";
    }
}