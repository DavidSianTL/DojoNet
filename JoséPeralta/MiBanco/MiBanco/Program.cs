using MiBanco.Data;
using MiBanco.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Usamos Singleton para que la misma instancia de daoClientes se use en toda la aplicación
builder.Services.AddSingleton<daoClientes>();

// Registramos el servicio de usuarios
builder.Services.AddSingleton<daoUsuarios>();

// Registramos el servicio de bitácora
builder.Services.AddSingleton<daoBitacora>();

// Registramos el servicio de pagos como daoPagos
builder.Services.AddSingleton<daoPagos>();

// Registramos el servicio de cuentas como daoCuentas
builder.Services.AddSingleton<daoCuentas>();

// Registramos el servicio de cuentas como daoSucursales
builder.Services.AddSingleton<daoSucursales>();

// Registramos el servicio de cuentas como daoRoles
builder.Services.AddSingleton<daoRoles>();

// Registramos el servicio de bitácora como IBitacoraService
builder.Services.AddSingleton<IBitacoraService, BitacoraService>();

// Registramos el servicio de JWT Authentication
builder.Services.AddSingleton<JWTService>();

// Extraemos la configuración del JWT 
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

// Configuramos la autenticación JWT
builder.Services.AddAuthentication(options =>
{
    // Establecemos el esquema de autenticación por defecto
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Añadimos el esquema de autenticación JWT Bearer
.AddJwtBearer(options =>
{
    // Configuramos las opciones de validación del token JWT
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Validamos el emisor, el auditorio, la vida útil y la clave de firma del token    
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Emisor"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Clave"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Le indicamos que use la autenticación JWT
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
