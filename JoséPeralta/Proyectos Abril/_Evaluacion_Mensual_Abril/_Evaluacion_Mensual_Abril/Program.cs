using _Evaluacion_Mensual_Abril.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Se agrega para el manejo de sesiones y caché
builder.Services.AddDistributedMemoryCache();

// Service to consume the API DummyJSON
// Siempre que se tenga una clase que va a hacer llamadas HTTP externas (APIs REST, SOAP, etc.), lo ideal es inyectar el HttpClient usando AddHttpClient.
builder.Services.AddHttpClient<_Evaluacion_Mensual_Abril.Services.FakeStore.FSProductService>();

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

// Activa las páginas personalizadas según el código de estado (ej. 404)
app.UseStatusCodePagesWithReExecute("/Error/Error", "?statusCode={0}");

app.UseRouting();

//app.UseAuthorization();

// Activar el uso de sesiones
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
