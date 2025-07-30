using Microsoft.Extensions.DependencyInjection;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Infrastructure;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.Usuario;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Manejo de sesiones y caché
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Registro de modelo de sesión
builder.Services.AddSingleton<UsuarioViewModel>();

// Configuración para el envío de correos electrónicos (Mailjet)
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("MailjetSettings"));
builder.Services.AddTransient<EmailService>();

// Cadena de conexión
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registro de DAOs
builder.Services.AddScoped<daoDepartamentoWSAsync>(_ => new daoDepartamentoWSAsync(connectionString));
builder.Services.AddScoped<daoDepartamentosEmpresaWSAsync>(_ => new daoDepartamentosEmpresaWSAsync(connectionString));
builder.Services.AddScoped<daoModulo>(_ => new daoModulo(connectionString));
builder.Services.AddScoped<daoModuloSistema>(_ => new daoModuloSistema(connectionString));
builder.Services.AddScoped<daoEmpleadoWSAsync>(_ => new daoEmpleadoWSAsync(connectionString));
builder.Services.AddScoped<daoEstadoWSAsync>(_ => new daoEstadoWSAsync(connectionString));
builder.Services.AddScoped<daoLogWSAsync>(_ => new daoLogWSAsync(connectionString));
builder.Services.AddScoped<daoPermisosWSAsync>(_ => new daoPermisosWSAsync(connectionString));
builder.Services.AddScoped<daoRolesWSAsync>(_ => new daoRolesWSAsync(connectionString));
builder.Services.AddScoped<daoRolPermisosWSAsync>(_ => new daoRolPermisosWSAsync(connectionString));
builder.Services.AddScoped<daoSistemaWSAsync>(_ => new daoSistemaWSAsync(connectionString));
builder.Services.AddScoped<daoTokenUsuario>(_ => new daoTokenUsuario(connectionString));
builder.Services.AddScoped<daoUsuarioWSAsync>(_ => new daoUsuarioWSAsync(connectionString));
builder.Services.AddScoped<daoUsuariosRolWSAsync>(_ => new daoUsuariosRolWSAsync(connectionString));
builder.Services.AddScoped<daoBitacoraWSAsync>(_ => new daoBitacoraWSAsync(connectionString));
builder.Services.AddScoped<daoSolicitudesAsync>(_ => new daoSolicitudesAsync(connectionString));

// Registro de servicios
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<IBitacoraService, BitacoraService>();
builder.Services.AddScoped<IEstadoService, EstadoService>();
builder.Services.AddScoped<IConnectionService, ConnectionService>(_ => new ConnectionService(connectionString));

// Inyectamos el servicio de paises
builder.Services.AddHttpClient<ICountryService, CountryService>();

//Registro de DepartamentosEmpresaController
builder.Services.AddScoped<daoEmpresaWSAsync>(_ => new daoEmpresaWSAsync(connectionString));

//Registro de EmpleadosDepartamentoController
builder.Services.AddScoped<daoEmpleadosEmpresaDepartamentoWSAsync>(_ => new daoEmpleadosEmpresaDepartamentoWSAsync(connectionString));

//Registro de SistemasEmpresasController
builder.Services.AddScoped<daoSistemasEmpresaWSAsync>(_ => new daoSistemasEmpresaWSAsync(connectionString));


//Daos de bitacora 
builder.Services.AddScoped<daoBitacoraWSAsync>(_ => new daoBitacoraWSAsync(connectionString));
builder.Services.AddScoped<daoLogWSAsync>(_ => new daoLogWSAsync(connectionString));
builder.Services.AddScoped<daoUsuariosRolWSAsync>(_ => new daoUsuariosRolWSAsync(connectionString));
builder.Services.AddScoped<ILoggingService, LoggingService>();

// Configuración de acceso denegado
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Home/AccesoDenegado";
});

var app = builder.Build();

// Middleware de entorno
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Middleware general
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "dashboard",
    pattern: "Dashboard/{action=Dashboard}/{id?}",
    defaults: new { controller = "Dashboard" });

app.Run();
