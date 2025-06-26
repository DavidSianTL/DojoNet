using SoapCore;
//using UsuariosAPISOAP.Services.v1;
//using UsuariosAPISOAP.Interfaces.v1;
//using UsuariosAPISOAP.Services.v2;
//using UsuariosAPISOAP.Interfaces.v2;
using UsuariosAPISOAP.Services.v3;
using UsuariosAPISOAP.Interfaces.v3;
using UsuariosAPISOAP.Services;
using UsuariosAPISOAP.Interfaces;
using Microsoft.EntityFrameworkCore;
using UsuariosAPISOAP.Data;
using Serilog;
using UsuariosAPISOAP.Middleware;
//TOKEN
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;




var builder = WebApplication.CreateBuilder(args);
//***********
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: "Logs/log-.txt",                  // formato log-2025-06-24.txt
        rollingInterval: RollingInterval.Day,   // archivo por día
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();
builder.Host.UseSerilog();
//*****
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddScoped<IUsuarioServiceEF, UsuarioServiceEF>();
builder.Services.AddRouting();
builder.Services.AddControllers();
//token
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "UsuariosAPISOAP",
            ValidAudience = "SessionUsuariosAPISOAP",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("W1(1@V3sVP3RS3(437@P@4W1(1@V3sVP"))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
//TOKEN

var app = builder.Build();



app.UseRouting();

app.UseMiddleware<LogMiddleware>();//inyectamos el Middleware
//Token
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{

    //endpoints.UseSoapEndpoint<IUsuarioService>("/UsuarioService.svc", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer);

    //endpoints.UseSoapEndpoint<IUsuarioServiceM>("/v1/UsuarioService.svc", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer);
    
   // endpoints.UseSoapEndpoint<IUsuarioServiceEF>("/v2/UsuarioService.svc", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer);

    endpoints.UseSoapEndpoint<IUsuarioServiceEF>("/v3/UsuarioService.svc", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer);


});

app.MapControllers();

app.Run();