using _Evaluacion_Mensual_Abril.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Se agrega para el manejo de sesiones y caché
builder.Services.AddDistributedMemoryCache();

//Configurando la Sesion
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});

// Para el manejo de la autenticación
builder.Services.AddSingleton<UserViewModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configurar el pipeline de la aplicación
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
