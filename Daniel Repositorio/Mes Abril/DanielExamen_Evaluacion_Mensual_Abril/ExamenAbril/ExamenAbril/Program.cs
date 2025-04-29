var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Habilitacion de servicios
// Agregamos el servicio de sesiones para poder guardar datos entre peticiones
var app = builder.Build();

// Si la aplicación no esta en desarrollo devuelve pagina de error.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Habilita el uso de sesiones para el login.
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

