using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TiendaCarritoPasarelaSmeall.Data;
using TiendaCarritoPasarelaSmeall.Models;
using TiendaCarritoPasarelaSmeall.Services;
using TiendaCarritoPasarelaSmeall.Validators;

var builder = WebApplication.CreateBuilder(args);

// ===== Configuración de seguridad =====
var sec = new SecurityOptions
{
    JwtIssuer = "smeall-api",
    JwtAudience = "smeall-clients",
    JwtSigningKey = "CAMBIA_ESTA_CLAVE_LARGA_Y_SEGURA_32+chars", // cámbiala en producción
    AccessTokenMinutes = 20,
    RefreshIdleMinutes = 20,
    RefreshAbsoluteHours = 8,
    MaxFailedAttempts = 5,
    LockoutMinutes = 15,
    PasswordMinLength = 10,
    PasswordRequireUpper = true,
    PasswordRequireLower = true,
    PasswordRequireDigit = true,
    PasswordRequireSpecial = true
};
builder.Services.AddSingleton(sec);

// ===== EF Core con SQL Server =====
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// ===== FluentValidation =====
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

// ===== Servicios de seguridad =====
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// ===== JWT =====
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

// ===== Swagger (para pruebas) =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese: Bearer {token}"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ===== Seed usuario de prueba (admin) =====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

    if (!db.Users.Any(u => u.UserName == "admin"))
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

// ===== Middlewares =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
