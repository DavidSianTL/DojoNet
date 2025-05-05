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

    public IActionResult Privacy()
    {
        return View();
    }

    /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string usuario)
    {
        var errorViewModel = new ErrorViewModel 
        { 
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, 
            Usuario = usuario 
        };

        // Guardo en una bitacora el error
        var rutaBitacora = $"{Environment.CurrentDirectory}\\Logs\\{usuario}.txt";
        System.IO.File.AppendAllText(rutaBitacora, $"{DateTime.Now} - {errorViewModel.RequestId}\r\n");

        return View(errorViewModel);
    }*/


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }



    
}
