using MiBanco.Data;
using MiBanco.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Usamos Singleton para que la misma instancia de daoClientes se use en toda la aplicaci�n
builder.Services.AddSingleton<daoClientes>();

// Registramos el servicio de bit�cora
builder.Services.AddSingleton<daoBitacora>();

// Registramos el servicio de bit�cora como IBitacoraService
builder.Services.AddSingleton<IBitacoraService, BitacoraService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
