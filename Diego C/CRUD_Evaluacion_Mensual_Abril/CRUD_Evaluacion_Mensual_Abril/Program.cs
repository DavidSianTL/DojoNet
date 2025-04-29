using CRUD_Evaluacion_Mensual_Abril.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuración para sesiones
builder.Services.AddDistributedMemoryCache();  // Usamos memoria para almacenar los datos de la sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Establece el tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Hace la cookie accesible solo desde el servidor
    options.Cookie.IsEssential = true; // Hace que la cookie sea esencial para la aplicación
});

// Inyección de dependencias
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IServicioSesion, ValidacionSesionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar el uso de sesiones
app.UseSession(); // Importante para habilitar la sesión en la aplicación

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"); // Ruta predeterminada para la aplicación

app.Run();
