using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaCarritoPasarelaSmeall.DTOs;
using TiendaCarritoPasarelaSmeall.Services;

namespace TiendaCarritoPasarelaSmeall.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;
        public AuthController(AuthService auth) { _auth = auth; }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            try
            {
                var resp = await _auth.LoginAsync(req.UserName, req.Password);
                if (resp == null) return Unauthorized(new { message = "Credenciales inválidas o cuenta bloqueada temporalmente" });
                return Ok(resp);
            }
            catch (InvalidOperationException ex) when (ex.Message.StartsWith("LOCKED"))
            {
                return StatusCode(423, new { message = ex.Message }); // 423 Locked
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
        {
            var resp = await _auth.RefreshAsync(req.RefreshToken);
            if (resp == null) return Unauthorized(new { message = "Refresh token inválido, vencido o inactivo" });
            return Ok(resp);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshRequest req)
        {
            var userId = GetUserIdFromAccess();
            var ok = await _auth.LogoutAsync(userId, req.RefreshToken);
            return ok ? Ok() : BadRequest();
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest req)
        {
            var userId = GetUserIdFromAccess();
            try
            {
                var ok = await _auth.ChangePasswordAsync(userId, req.CurrentPassword, req.NewPassword);
                return ok ? Ok(new { message = "Contraseña actualizada. Se cerraron las sesiones" })
                          : BadRequest(new { message = "Contraseña actual incorrecta" });
            }
            catch (InvalidOperationException ex) when (ex.Message == "INVALID_PASSWORD_POLICY")
            {
                return BadRequest(new { message = "La nueva contraseña no cumple la política" });
            }
        }

        private Guid GetUserIdFromAccess()
        {
            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                      ?? throw new UnauthorizedAccessException();
            return Guid.Parse(sub);
        }
    }
}
