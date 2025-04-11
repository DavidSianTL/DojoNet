using Practica_JavaScript.Models;
using Practica_JavaScript.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Se agrega para el manejo de sesiones y cach�
builder.Services.AddDistributedMemoryCache();

//Configurando la Sesion
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});

builder.Services.AddSingleton<UsuarioViewModel>();

// Se agrega el servicio de BitacoraService
// Se registra el servicio de BitacoraService como un servicio singleton
builder.Services.AddSingleton<BitacoraService>(provider =>
    new BitacoraService("bitacoraUsuario.txt"));

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configurar el pipeline de la aplicaci�n
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

// Activar el uso de sesiones
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
