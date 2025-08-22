using Microsoft.EntityFrameworkCore;
using proyectoWEBSITESmeall.Models; // BbddSmeallContext

var builder = WebApplication.CreateBuilder(args);

// MVC + API
builder.Services.AddControllersWithViews();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// === CONEXIÓN A bbddSmeall (MISMA CADENA PARA EF CORE Y ADO/DAPPER) ===
var cs = builder.Configuration.GetConnectionString("bbddSmeallConn")
         ?? throw new InvalidOperationException("Falta ConnectionStrings:bbddSmeallConn en appsettings.json");


builder.Services.AddDbContext<BbddSmeallContext>(opt => opt.UseSqlServer(cs));


builder.Services.AddScoped<
    proyectoWEBSITESmeall.Infrastructure.IDbConnectionFactory,
    proyectoWEBSITESmeall.Infrastructure.SqlConnectionFactory>();


builder.Services.AddScoped<proyectoWEBSITESmeall.Repositories.TiendaRepository>();
builder.Services.AddScoped<proyectoWEBSITESmeall.Repositories.CarritoRepository>();
builder.Services.AddScoped<proyectoWEBSITESmeall.Repositories.OrdenRepository>();
builder.Services.AddScoped<proyectoWEBSITESmeall.Services.TiendaService>();
builder.Services.AddScoped<proyectoWEBSITESmeall.Services.CarritoService>();
builder.Services.AddScoped<proyectoWEBSITESmeall.Services.OrdenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
