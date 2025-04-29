using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using Practica3.Models;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------
// CONFIGURACIÓN DE SERILOG
// Inicializa Serilog para registrar en consola y archivo por día en /Logs
// -----------------------------------------------------------------------
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Solo muestra Warning o superior de Microsoft
    .MinimumLevel.Information() // A partir de Information para tu app
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Establece Serilog como proveedor de logs para toda la aplicación
builder.Host.UseSerilog();

// -----------------------------------------------------------------------
// SERVICIOS
// -----------------------------------------------------------------------
builder.Services.AddControllersWithViews();

// Habilita manejo de sesiones (cookies necesarias)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Servicio personalizado para manejo de usuarios
builder.Services.AddSingleton<UsuarioServicio>();

var app = builder.Build();

// -----------------------------------------------------------------------
// MIDDLEWARE Y PIPELINE
// -----------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Fuerza HTTPS en producción
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Activa uso de sesión

// -----------------------------------------------------------------------
// RUTEO
// -----------------------------------------------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
