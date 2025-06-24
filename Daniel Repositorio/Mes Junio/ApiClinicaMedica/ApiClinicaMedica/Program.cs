using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;  
using ApiClinicaMedica.Data;
using ApiClinicaMedica.Services;
using ApiClinicaMedica.Middleware;
using ApiClinicaMedica.Daos;
using ApiClinicaMedica.Dao;




var builder = WebApplication.CreateBuilder(args);

// Leer configuración desde appsettings.json (clave, base de datos, etc.)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

object value = builder.Host.UseSerilog(); // Habilitar Serilog

// ======== Base de datos ========
builder.Services.AddDbContext<ClinicaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClinicaConnection")));

// ======== JWT ========
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Clave"]))
    };
});

builder.Services.AddSingleton<JwtService>();

// ======== DAOs ========
builder.Services.AddScoped<EspecialidadDAO>();
builder.Services.AddScoped<MedicoDAO>();
builder.Services.AddScoped<PacienteDAO>();
builder.Services.AddScoped<CitaDao>();

// ======== Versionamiento ========
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = false;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader(); // /api/v1/controller
});

// ======== Swagger y Controladores ========
// Aquí se agrega la configuración para evitar ciclos JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true; // Opcional, para formato legible
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ======== Pipeline HTTP ========
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // JWT
app.UseAuthorization();

// Middleware personalizado de auditoría
app.UseMiddleware<AuditoriaMiddleware>();

app.MapControllers();
app.Run();
