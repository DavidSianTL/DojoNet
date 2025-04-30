using System.Globalization;
using ExamDaniel.Models;
using ExamDaniel.Servicios;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile("config.json", optional: true, reloadOnChange: true);


builder.Services.Configure<AppConfig>(
    builder.Configuration.GetSection("AppConfig"));


builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpClient<ApiRestService>();

var app = builder.Build();


var cfg = app.Services.GetRequiredService<IOptions<AppConfig>>().Value;
var cultura = new CultureInfo(cfg.Idioma);
CultureInfo.DefaultThreadCurrentCulture = cultura;
CultureInfo.DefaultThreadCurrentUICulture = cultura;

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();




