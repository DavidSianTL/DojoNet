using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Trabajo_APIRest.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UsuariosApi.Services;
using Trabajo_APIRest.DAO;
using UsuariosApi.DAO;

var builder = WebApplication.CreateBuilder(args);


// Leer configuración desde appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddSingleton<DbConnection>();
builder.Services.AddScoped<daoPacienteWSAsync>();
builder.Services.AddScoped<daoMedicoWSAsync>();
builder.Services.AddScoped<daoEspecialidadWSASync>();
builder.Services.AddScoped<daoCitaWSAsync>();
builder.Services.AddScoped<daoAutorizacionWSAsync>();
builder.Services.AddScoped<daoUsuariosWSAsync>();

builder.Services.AddControllers();

//CONFIGURACIÓN PARA VERSIONAMIENTO
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = false;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;

    // Esto es CLAVE para que lea la versión desde la URL
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});



var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    // Configurar los tipos de claims que se usarán para la identidad
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Usuario"],
        ValidAudience = jwtSettings["Sesion"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Clave"])),
        // Configuración para que User.Identity.Name funcione correctamente
        NameClaimType = ClaimTypes.Name,
        // Asegurar que el token no haya expirado
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            var path = context.Request.Path;
            var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("401 - Acceso no autenticado a {Path} desde IP {IP}", path, ip);
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            var path = context.Request.Path;
            var user = context.HttpContext.User.Identity?.Name ?? "desconocido";
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("403 - Usuario {User} no autorizado para acceder a {Path}", user, path);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddSingleton<JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
