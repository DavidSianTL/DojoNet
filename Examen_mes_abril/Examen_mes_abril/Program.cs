using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Examen_mes_abril.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Se agrega para el manejo de variable de session
builder.Services.AddDistributedMemoryCache();


//Configurando la Sesion
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});

builder.Services.AddSingleton<UsuarioServicioModel>();
builder.Services.AddSingleton<ProductoServicioModel>();

var app = builder.Build();

// Usar el middleware para manejar los errores
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Página de errores para desarrollo
}
else
{
    // Manejo de errores en producción
    app.UseExceptionHandler("/Home/Error"); // Redirigir a la acción "Error" en el controlador "Home"
    app.UseHsts(); // Habilitar HTTP Strict Transport Security
}

app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

//Activar el uso de sesiones
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();

