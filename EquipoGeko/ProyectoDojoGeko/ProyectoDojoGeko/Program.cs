using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.Usuario;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Se agrega para el manejo de sesiones y caché
builder.Services.AddDistributedMemoryCache();

//Configurando la Sesion
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Para el manejo de la autenticación
builder.Services.AddSingleton<UsuarioViewModel>();

// Configuración para el envío de correos electrónicos
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("MailSettings"));

// Registramos el servicio de EmailService para inyección de dependencias
builder.Services.AddTransient<EmailService>(); 


builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Home/AccesoDenegado";
}); 

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configurar el pipeline de la aplicación
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Configuración para manejar códigos de estado HTTP (como 404)
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Usamos "UseSession" para habilitar las sesiones por Usuario
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "dashboard",
    pattern: "Dashboard/{action=Dashboard}/{id?}",
    defaults: new { controller = "Dashboard" });

app.Run();