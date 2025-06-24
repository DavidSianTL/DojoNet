using ClinicaApi.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFile("Logs/clinica-{Date}.log");//guardar los logs en texto


// 🔐 Configuración de autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers(); // 💡 esencial para usar controladores
builder.Services.AddOpenApi();     // Swagger

// 🧠 Registro de DAOs con soporte para ILogger
builder.Services.AddScoped<CitaDao>();
builder.Services.AddScoped<MedicoDao>();
builder.Services.AddScoped<PacienteDao>();
builder.Services.AddScoped<EspecialidadDao>();
builder.Services.AddScoped<UsuarioDao>();

var app = builder.Build();

// 🌐 Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // swagger
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

string hash = BCrypt.Net.BCrypt.HashPassword("admin123");
Console.WriteLine(hash);

app.UseDefaultFiles(); // Habilita index.html como landing
app.UseStaticFiles();  // Permite servir archivos estáticos

app.MapControllers(); // 🚀 Controladores

app.Run();
