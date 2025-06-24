using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using HojadeTrabajoAPI_REST.DAO;
using HojadeTrabajoAPI_REST.DATA;
using Asp.Versioning;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using HojadeTrabajoAPI_REST.Services;
using Serilog.Settings.Configuration;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Leer configuración desde appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


// Add services to the container.
builder.Services.AddSingleton<DbConnection>();
builder.Services.AddScoped<daoPacienteAsync>();
builder.Services.AddScoped<daoMedicoAsync>();
builder.Services.AddScoped<daoEspecialidadAsync>();
builder.Services.AddScoped<daoCitaAsync>();
builder.Services.AddScoped<daoUsuarioAsync>();

builder.Services.AddControllers();

//CONFIGURACIÒN PARA VERSIONAMIENTO
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
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//Sirve para ver una vista de html
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
