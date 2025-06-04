using wbSistemaSeguridadMVC.Data;
using wbSistemaSeguridadMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();//AGREGAR

////INYECTANDO LA CADENA DE SETTINGS
//string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//// Registrar servicios
//builder.Services.AddSingleton(new cnnConexionAsync(connectionString));
//builder.Services.AddScoped<daoSistemaAsyncWS>();
//builder.Services.AddControllersWithViews();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();//AGREGAR

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
