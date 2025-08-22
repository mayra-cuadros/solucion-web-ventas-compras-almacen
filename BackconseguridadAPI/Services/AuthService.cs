using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using TiendaCarritoPasarelaSmeall.Data;
using TiendaCarritoPasarelaSmeall.DTOs;
using TiendaCarritoPasarelaSmeall.Models;

namespace TiendaCarritoPasarelaSmeall.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _hasher;
        private readonly JwtTokenService _jwt;
        private readonly SecurityOptions _opts;

        public AuthService(AppDbContext db, IPasswordHasher<User> hasher, JwtTokenService jwt, IOptions<SecurityOptions> opts)
        {
            _db = db; _hasher = hasher; _jwt = jwt; _opts = opts.Value;
        }

        // HU-1: Login (bloqueo por intentos)
        public async Task<LoginResponse?> LoginAsync(string userName, string password)
        {
            var user = await _db.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null) return null;

            if (user.LockoutEndUtc.HasValue && user.LockoutEndUtc.Value > DateTimeOffset.UtcNow)
                throw new InvalidOperationException($"LOCKED|Cuenta bloqueada hasta {user.LockoutEndUtc:O}");

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (verify == PasswordVerificationResult.Failed)
            {
                user.FailedLoginCount += 1;
                user.LastFailedLoginUtc = DateTimeOffset.UtcNow;
                if (user.FailedLoginCount >= _opts.MaxFailedAttempts)
                {
                    user.LockoutEndUtc = DateTimeOffset.UtcNow.AddMinutes(_opts.LockoutMinutes);
                }
                await _db.SaveChangesAsync();
                return null;
            }

            user.FailedLoginCount = 0;
            user.LockoutEndUtc = null;

            var (access, exp) = _jwt.CreateAccessToken(user);
            var refresh = CreateRefreshToken();

            var now = DateTimeOffset.UtcNow;
            var rt = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = Hash(refresh),
                CreatedAt = now,
                LastUsedAt = now,
                ExpiresAt = now.AddHours(_opts.RefreshAbsoluteHours)
            };
            user.RefreshTokens.Add(rt);
            await _db.SaveChangesAsync();

            return new LoginResponse(access, refresh, exp);
        }

        // HU-2: Refresh usando solo el refresh token (resuelve user internamente)
        public async Task<LoginResponse?> RefreshAsync(string refreshToken)
        {
            var hash = Hash(refreshToken);
            var now = DateTimeOffset.UtcNow;

            var rt = await _db.RefreshTokens.Include(r => r.User)
                .FirstOrDefaultAsync(r => r.TokenHash == hash);

            if (rt == null || rt.RevokedAt != null || rt.ExpiresAt <= now)
                return null;

            // Inactividad
            if (rt.LastUsedAt.AddMinutes(_opts.RefreshIdleMinutes) <= now)
                return null;

            rt.LastUsedAt = now;
            var (access, exp) = _jwt.CreateAccessToken(rt.User!);
            await _db.SaveChangesAsync();
            return new LoginResponse(access, refreshToken, exp);
        }

        public async Task<bool> LogoutAsync(Guid userId, string refreshToken)
        {
            var hash = Hash(refreshToken);
            var rt = await _db.RefreshTokens
                .FirstOrDefaultAsync(r => r.TokenHash == hash && r.UserId == userId && r.RevokedAt == null);

            if (rt == null) return false;
            rt.RevokedAt = DateTimeOffset.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        // HU-3: Cambio de contraseña (revoca todos los refresh tokens)
        public async Task<bool> ChangePasswordAsync(Guid userId, string current, string next)
        {
            var user = await _db.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, current);
            if (verify == PasswordVerificationResult.Failed) return false;

            if (!ValidatePasswordPolicy(next))
                throw new InvalidOperationException("INVALID_PASSWORD_POLICY");

            user.PasswordHash = _hasher.HashPassword(user, next);
            user.PasswordChangedAtUtc = DateTimeOffset.UtcNow;

            foreach (var rt in user.RefreshTokens.Where(r => r.RevokedAt == null))
                rt.RevokedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        private static string CreateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

        private bool ValidatePasswordPolicy(string pwd)
        {
            if (pwd.Length < _opts.PasswordMinLength) return false;
            if (_opts.PasswordRequireUpper && !pwd.Any(char.IsUpper)) return false;
            if (_opts.PasswordRequireLower && !pwd.Any(char.IsLower)) return false;
            if (_opts.PasswordRequireDigit && !pwd.Any(char.IsDigit)) return false;
            if (_opts.PasswordRequireSpecial && !pwd.Any(c => !char.IsLetterOrDigit(c))) return false;
            return true;
        }
    }
}
