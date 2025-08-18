using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Demo_asignacionPuestos.Models;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Demo_asignacionPuestos.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "usuarios.json");

        if (!System.IO.File.Exists(rutaArchivo))
        {
            return NotFound("No se encontró el archivo usuarios.json");
        }

        var contenidoJson = System.IO.File.ReadAllText(rutaArchivo);

        var empleados = JsonSerializer.Deserialize<List<DependientesModel>>(contenidoJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return View(empleados);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
