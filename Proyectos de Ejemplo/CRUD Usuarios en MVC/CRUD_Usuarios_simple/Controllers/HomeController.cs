using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUD_Usuarios_simple.Models;

namespace CRUD_Usuarios_simple.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    // Hacer la lista estática para compartirla entre controladores
    private static List<UsuarioViewModel> _usuarios = new List<UsuarioViewModel>();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(_usuarios);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
