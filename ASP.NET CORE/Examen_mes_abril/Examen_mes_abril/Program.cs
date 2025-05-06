using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Examen_mes_abril.Models;
using Examen_mes_abril.Services;


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
builder.Services.AddScoped<ISoapDemoService, ServicioSOAPDemo>();

var app = builder.Build();

// Usar el middleware para manejar los errores
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error/500"); // Error 500 
    app.UseStatusCodePagesWithRedirects("/Home/Error/{0}"); // Otros errores
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
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

