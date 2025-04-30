using CRUD_Evaluacion_Mensual_Abril.Models;
using CRUD_Evaluacion_Mensual_Abril.Service;
using CRUD_Evaluacion_Mensual_Abril.Services;


var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Registrar el servicio UsuarioServicio en el contenedor de dependencias
builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<BitacoraService>();
builder.Services.AddScoped<ICalculadoraService, CalculadoraService>();
builder.Services.AddHttpClient<ProductosApiController>();
builder.Services.AddSingleton<ConfiguracionService>();


// Configurar la sesión y cache distribuido
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configurar la canalización de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Seguridad adicional para producción
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();  // Habilitar el uso de sesiones
app.UseSession();
app.UseMiddleware<SesionActivaMiddleware>();
// Configurar rutas
app.UseExceptionHandler("/Home/Error"); // para errores 500
app.UseStatusCodePagesWithReExecute("/Home/Error"); // para errores 404 y similares


app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();