using ClinicaMedicaAPIREST.Data.DAOs;
using ClinicaMedicaAPIREST.Security;
using ClinicaMedicaAPIREST.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add the DbConnectionService as a IDbConnectionService implementation
builder.Services.AddScoped<IDbConnectionService, DbConnectionService>();
builder.Services.AddScoped<daoPacientes>();
builder.Services.AddScoped<daoMedicos>();
builder.Services.AddScoped<daoCitas>();
builder.Services.AddScoped<daoUsuarios>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();


app.MapControllers();


app.Run();
