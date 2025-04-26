var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Usar el middleware para manejar los errores
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // P�gina de errores para desarrollo
}
else
{
    // Manejo de errores en producci�n
    app.UseExceptionHandler("/Home/Error"); // Redirigir a la acci�n "Error" en el controlador "Home"
    app.UseHsts(); // Habilitar HTTP Strict Transport Security
}

app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
