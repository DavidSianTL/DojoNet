using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.Usuario;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Se agrega para el manejo de sesiones y caché
builder.Services.AddDistributedMemoryCache();

//Configurando la Sesion
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Para el manejo de la autenticación
builder.Services.AddSingleton<UsuarioViewModel>();

// Configuración para el envío de correos electrónicos
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<EmailService>();

// Cadena de conexión para DAOs
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registro de DAOs utilizados en LoginController
builder.Services.AddScoped<daoTokenUsuario>(_ => new daoTokenUsuario(connectionString));
builder.Services.AddScoped<daoBitacoraWSAsync>(_ => new daoBitacoraWSAsync(connectionString));
builder.Services.AddScoped<daoUsuarioWSAsync>(_ => new daoUsuarioWSAsync(connectionString));
builder.Services.AddScoped<daoEmpleadoWSAsync>(_ => new daoEmpleadoWSAsync(connectionString));
builder.Services.AddScoped<daoUsuariosRolWSAsync>(_ => new daoUsuariosRolWSAsync(connectionString));
builder.Services.AddScoped<daoRolesWSAsync>(_ => new daoRolesWSAsync(connectionString));
builder.Services.AddScoped<daoRolPermisosWSAsync>(_ => new daoRolPermisosWSAsync(connectionString));

// Registro del DAO de logs y el servicio de logging
builder.Services.AddScoped<daoLogWSAsync>(_ => new daoLogWSAsync(connectionString));
builder.Services.AddScoped<ILoggingService, LoggingService>();

// Registro de DAOs utilizados en UsuariosController
builder.Services.AddScoped<daoUsuarioWSAsync>(_ => new daoUsuarioWSAsync(connectionString));
builder.Services.AddScoped<daoEmpleadoWSAsync>(_ => new daoEmpleadoWSAsync(connectionString));
builder.Services.AddScoped<daoBitacoraWSAsync>(_ => new daoBitacoraWSAsync(connectionString));


// Servicio de correos (si no lo tienes ya)
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<EmailService>();

// DAOs utilizados en EmpleadosController
builder.Services.AddScoped<daoEmpleadoWSAsync>(_ => new daoEmpleadoWSAsync(connectionString));
builder.Services.AddScoped<daoUsuarioWSAsync>(_ => new daoUsuarioWSAsync(connectionString));
builder.Services.AddScoped<daoRolesWSAsync>(_ => new daoRolesWSAsync(connectionString));
builder.Services.AddScoped<daoSistemaWSAsync>(_ => new daoSistemaWSAsync(connectionString));
builder.Services.AddScoped<daoBitacoraWSAsync>(_ => new daoBitacoraWSAsync(connectionString));
builder.Services.AddScoped<ILoggingService, LoggingService>();
//DAOs de UsuariosRolController
builder.Services.AddScoped<daoUsuariosRolWSAsync>(_ => new daoUsuariosRolWSAsync(connectionString));
builder.Services.AddScoped<daoUsuarioWSAsync>(_ => new daoUsuarioWSAsync(connectionString));
builder.Services.AddScoped<daoRolesWSAsync>(_ => new daoRolesWSAsync(connectionString));
builder.Services.AddScoped<daoBitacoraWSAsync>(_ => new daoBitacoraWSAsync(connectionString));
builder.Services.AddScoped<ILoggingService, LoggingService>();

//DAOs de Rolespermiso
builder.Services.AddScoped<daoRolesWSAsync>(_ => new daoRolesWSAsync(connectionString));
builder.Services.AddScoped<daoUsuariosRolWSAsync>(_ => new daoUsuariosRolWSAsync(connectionString));
builder.Services.AddScoped<daoBitacoraWSAsync>(_ => new daoBitacoraWSAsync(connectionString));
builder.Services.AddScoped<ILoggingService, LoggingService>();

// DAOs de PermisosController
builder.Services.AddScoped<daoPermisosWSAsync>(_ => new daoPermisosWSAsync(connectionString));
builder.Services.AddScoped<daoUsuariosRolWSAsync>(_ => new daoUsuariosRolWSAsync(connectionString));
builder.Services.AddScoped<daoBitacoraWSAsync>(_ => new daoBitacoraWSAsync(connectionString));
builder.Services.AddScoped<ILoggingService, LoggingService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Home/AccesoDenegado";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Configuración para manejar códigos de estado HTTP (como 404)
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Usamos "UseSession" para habilitar las sesiones por Usuario
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "dashboard",
    pattern: "Dashboard/{action=Dashboard}/{id?}",
    defaults: new { controller = "Dashboard" });

app.Run();
