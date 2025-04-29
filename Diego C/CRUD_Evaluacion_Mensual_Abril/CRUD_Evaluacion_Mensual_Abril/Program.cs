using CRUD_Evaluacion_Mensual_Abril.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuraci�n para sesiones
builder.Services.AddDistributedMemoryCache();  // Usamos memoria para almacenar los datos de la sesi�n
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Establece el tiempo de expiraci�n de la sesi�n
    options.Cookie.HttpOnly = true; // Hace la cookie accesible solo desde el servidor
    options.Cookie.IsEssential = true; // Hace que la cookie sea esencial para la aplicaci�n
});

// Inyecci�n de dependencias
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
app.UseSession(); // Importante para habilitar la sesi�n en la aplicaci�n

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"); // Ruta predeterminada para la aplicaci�n

app.Run();
