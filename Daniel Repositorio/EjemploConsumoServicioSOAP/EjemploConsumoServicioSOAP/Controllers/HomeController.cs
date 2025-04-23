using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EjemploConsumoServicioSOAP.Models;
using EjemploConsumoServicioSOAP.Service;

namespace EjemploConsumoServicioSOAP.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICountryInfo _countryInfo;

    public HomeController(ILogger<HomeController> logger, ICountryInfo countryInfo)
    {
        _logger = logger;
        _countryInfo = countryInfo;
    }

    public IActionResult Menu()
    {
        return View();
    }



    public IActionResult Index()
    {
        return View();
    }


    //Accion para obtener la capital de un pais
    public async Task<IActionResult> MostrarCapital()
    {
        string resultado = string.Empty;
        try
        {
            resultado = await _countryInfo.CountryInfoService();
        }
        catch (Exception ex)
        {

            resultado = $"Error al consultar: {ex.Message}";
        }

        //Retornar resultado a la vista
        ViewBag.Resultado = resultado;
        return View("Index");

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
