using API2.Data;
using API2.Data.DAOs;
using API2.Data;
using API2.Data.DAOs;
using Microsoft.EntityFrameworkCore;
using API2.Data.DAOs;
using API2.Data.DAOs;
using API2.Data.DAOs;
using API2.Data.DAOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;



var builder = WebApplication.CreateBuilder(args);
// Clave secreta para JWT (usada para firmar y validar)
var key = "EstaEsMiClaveSuperSecretaParaJWT!123";
// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Cadena de conexión desde appsettings.json
builder.Services.AddDbContext<ClinicaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de dependencias para los DAOs
builder.Services.AddScoped<PacienteDAO>();
builder.Services.AddScoped<MedicoDAO>();
builder.Services.AddScoped<CitaDAO>();
builder.Services.AddScoped<EspecialidadDAO>();

// Controllers y JSON Settings
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Swagger para pruebas API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
var app = builder.Build();



// Swagger Middleware (solo en desarrollo)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
