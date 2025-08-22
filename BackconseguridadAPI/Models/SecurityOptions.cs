namespace TiendaCarritoPasarelaSmeall.Models
{
    public class SecurityOptions
    {
        public string JwtIssuer { get; set; } = string.Empty;
        public string JwtAudience { get; set; } = string.Empty;
        public string JwtSigningKey { get; set; } = string.Empty;

        public int AccessTokenMinutes { get; set; }
        public int RefreshIdleMinutes { get; set; }
        public int RefreshAbsoluteHours { get; set; }

        public int MaxFailedAttempts { get; set; }
        public int LockoutMinutes { get; set; }

        public int PasswordMinLength { get; set; }
        public bool PasswordRequireUpper { get; set; }
        public bool PasswordRequireLower { get; set; }
        public bool PasswordRequireDigit { get; set; }
        public bool PasswordRequireSpecial { get; set; }
    }
}
