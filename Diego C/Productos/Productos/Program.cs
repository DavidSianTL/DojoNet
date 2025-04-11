using Productos.Models;


var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Registrar el servicio UsuarioServicio en el contenedor de dependencias
builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<BitacoraServicio>();
// A�adir esta l�nea

// Configurar la sesi�n y cach� distribuido
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configurar la canalizaci�n de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Seguridad adicional para producci�n
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();  // Habilitar el uso de sesiones
app.UseSession();
app.UseMiddleware<SesionActivaMiddleware>();
// Configurar rutas


app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();