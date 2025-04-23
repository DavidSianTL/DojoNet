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

    public IActionResult Index()
    {
        return View();
    }

    // Acción para obtener la capital de un país (actualizada)
    public async Task<IActionResult> MostrarCapital(string codigoPais = "GT") // Valor por defecto: Guatemala
    {
        string resultado;
        try
        {
            resultado = await _countryInfo.ConsultarCapitalPorCodigo(codigoPais);
        }
        catch (Exception ex)
        {
            resultado = $"Error al consultar: {ex.Message}";
        }

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