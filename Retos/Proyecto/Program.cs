using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllersWithViews(); // Habilitar controladores con vistas (MVC)
builder.Services.AddSession(); // Activar sesiones
builder.Services.AddHttpContextAccessor(); // Proveer acceso al contexto HTTP

var app = builder.Build();

// Configurar middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // Middleware para sesiones
app.UseAuthorization(); // Autorizar rutas si es necesario

// Configurar las rutas predeterminadas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
