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

// Usamos "UseSession" para habilitar las sesiones por Usuario
app.UseSession();

// app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
