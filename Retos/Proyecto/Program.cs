using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;


var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); 

var app = builder.Build();

// ...
app.UseStaticFiles();
app.UseRouting();

app.UseSession(); // Esto es obligatorio para que funcione HttpContext.Session

app.UseAuthorization();

// ...
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();

public static class Logger
{
    private static string logFilePath = "logs.txt";

    public static void Log(string usuario, string mensaje)
    {
        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Usuario: {usuario} - Mensaje: {mensaje}";
        File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
    }
}