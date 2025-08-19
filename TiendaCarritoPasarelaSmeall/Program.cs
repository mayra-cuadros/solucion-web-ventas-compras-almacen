using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TiendaCarritoPasarelaSmeall.Data;
using TiendaCarritoPasarelaSmeall.Models;
using TiendaCarritoPasarelaSmeall.Services;
using TiendaCarritoPasarelaSmeall.Validators;

var builder = WebApplication.CreateBuilder(args);

// ===== Configuración de seguridad (puedes mover a appsettings) =====
builder.Services.Configure<SecurityOptions>(opts =>
{
    opts.JwtIssuer = "smeall-api";
    opts.JwtAudience = "smeall-clients";
    opts.JwtSigningKey = "CAMBIA_ESTA_CLAVE_LARGA_Y_SEGURA_32+chars";
    opts.AccessTokenMinutes = 20;
    opts.RefreshIdleMinutes = 20;
    opts.RefreshAbsoluteHours = 8;
    opts.MaxFailedAttempts = 5;
    opts.LockoutMinutes = 15;
    opts.PasswordMinLength = 10;
    opts.PasswordRequireUpper = true;
    opts.PasswordRequireLower = true;
    opts.PasswordRequireDigit = true;
    opts.PasswordRequireSpecial = true;
});

// ===== EF Core =====
// Rápido: InMemory (cámbialo luego a SQL Server)
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseInMemoryDatabase("SmeallAuthDb");
    // Para SQL Server:
    // opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// ===== FluentValidation =====
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

// ===== Servicios de seguridad =====
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// ===== JWT =====
var sp = builder.Services.BuildServiceProvider();
var sec = sp.GetRequiredService<IOptions<SecurityOptions>>().Value;
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sec.JwtSigningKey));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = sec.JwtIssuer,
            ValidAudience = sec.JwtAudience,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// ===== Seed usuario de prueba =====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

    if (!db.Users.Any())
    {
        var u = new User
        {
            Id = Guid.NewGuid(),
            UserName = "admin",
            PasswordChangedAtUtc = DateTimeOffset.UtcNow
        };
        u.PasswordHash = hasher.HashPassword(u, "Admin#2024!");
        db.Users.Add(u);
        db.SaveChanges();
    }
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
