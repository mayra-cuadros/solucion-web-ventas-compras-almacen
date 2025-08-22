namespace TiendaCarritoPasarelaSmeall.DTOs
{
    public record LoginRequest(string UserName, string Password);
    public record LoginResponse(string AccessToken, string RefreshToken, DateTimeOffset ExpiresAt);
    public record RefreshRequest(string RefreshToken);
    public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
}
