using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Manejo_de_Errores.Models;

namespace Manejo_de_Errores.Controllers;

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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? statusCode)
{
    if (statusCode == 404)
    {
        ViewData["ErrorMessage"] = "La página solicitada no fue encontrada.";
    }
    else
    {
        ViewData["ErrorMessage"] = "Ocurrió un error inesperado.";
    }

    return View();
}

}
