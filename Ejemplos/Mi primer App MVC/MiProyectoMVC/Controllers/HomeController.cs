using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiProyectoMVC.Models;

namespace MiProyectoMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registro(Usuario usuario)
    {
        try
        {
            if (ModelState.IsValid)
            {
                ViewBag.Mensaje = "✅ Usuario registrado con éxito.";
                return View("Index", usuario);
            }
            else
            {
                ViewBag.Mensaje = "❌ Error: Algunos campos no son válidos.";
                return View("Index", usuario); // Se asegura de retornar la vista correcta
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error en Registro: {ex.Message}");
            ViewBag.Mensaje = "❌ Ocurrió un error inesperado.";
            return View("Index", usuario);
        }
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
