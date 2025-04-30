using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AppFuturista.Services;
using AppFuturista.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Agregar soporte para sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Registrar servicios
builder.Services.AddSingleton<ArchivoJsonService>();
builder.Services.AddSingleton<BitacoraService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<UsuarioService>();

// Registrar HttpClient para APIs
builder.Services.AddHttpClient<ApiRestService>();
builder.Services.AddHttpClient<ApiSoapService>();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error500");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicializar la configuración
using (var scope = app.Services.CreateScope())
{
    var archivoJsonService = scope.ServiceProvider.GetRequiredService<ArchivoJsonService>();
    var configuracion = await archivoJsonService.LeerConfiguracionAsync<ConfiguracionApp>("configuracion.json");
}

app.Run();
