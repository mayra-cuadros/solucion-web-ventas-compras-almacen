using System.ComponentModel.DataAnnotations;

namespace TiendaCarritoPasarelaSmeall.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // HU-1: bloqueo por intentos fallidos
        public int FailedLoginCount { get; set; }
        public DateTimeOffset? LockoutEndUtc { get; set; }
        public DateTimeOffset? LastFailedLoginUtc { get; set; }

        // HU-3: cambio de contraseña
        public DateTimeOffset? PasswordChangedAtUtc { get; set; }

        // Relación
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
