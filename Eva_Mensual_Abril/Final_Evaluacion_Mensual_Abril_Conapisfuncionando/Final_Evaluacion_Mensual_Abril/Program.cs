using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Final_Evaluacion_Mensual_Abril.Models;
using Final_Evaluacion_Mensual_Abril.Services;
using Proyecto1.Services;
using Evaluacion_Mensual_Abril.Models;
using Final_Evaluacion_Mensual_Abril.Filters;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor(); 


builder.Services.AddSingleton<BitacoraService>();
builder.Services.AddSingleton<ProductoServicio>();
builder.Services.AddScoped<FakeStoreService>();
builder.Services.AddSingleton<UsuarioServicio>();


builder.Services.AddDistributedMemoryCache();


builder.Services.AddHttpClient("FakeStoreAPI", client =>
{
    client.BaseAddress = new Uri("https://fakestoreapi.com/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});


builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<LoginAuthorizeAttribute>();  
    options.Filters.Add<BitacoraActionFilter>();    
    options.Filters.Add(new AutorizacionFilter());  
});

var app = builder.Build();


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


app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}"); // Login como pantalla inicial

app.Run();
