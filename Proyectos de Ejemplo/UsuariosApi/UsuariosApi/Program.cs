using Asp.Versioning;
using System.Data;
using UsuariosApi.DAO;
using UsuariosApi.Data;
using UsuariosApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DbConnection>();
builder.Services.AddScoped<daoUsuarios>();
builder.Services.AddScoped<daoUsuariosAsync>();
builder.Services.AddScoped<daoAuditoria>(); 

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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<AuditoriaMiddleware>();

app.MapControllers();

app.Run();
