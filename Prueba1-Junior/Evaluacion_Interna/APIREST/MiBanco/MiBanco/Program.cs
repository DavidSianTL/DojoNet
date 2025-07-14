using MiBanco.Data.DAOs;
using MiBanco.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IClienteDAO, ClienteDAO>(); 
builder.Services.AddScoped<IPagoDAO, PagoDAO>(); 
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPagoService, PagoService>();
builder.Services.AddScoped<ILogService, LogService>();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();
app.Run();