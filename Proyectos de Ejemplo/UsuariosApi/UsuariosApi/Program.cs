using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using UsuariosApi.DAO;
using UsuariosApi.Data;
using UsuariosApi.Middleware;
using UsuariosApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddSingleton<DbConnection>();
builder.Services.AddScoped<daoUsuarios>();
builder.Services.AddScoped<daoUsuariosAsync>();
builder.Services.AddScoped<daoAuditoria>();

builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<daoAuditoriaEF>();
builder.Services.AddScoped<daoUsuarioAsyncEF>();

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
});

builder.Services.AddSingleton<JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<AuditoriaMiddleware>();

app.MapControllers();

app.Run();
