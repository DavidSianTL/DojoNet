using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ConsumoServiciosSoap.Models;
using CountryInfoService;
using ConsumoServiciosSoap.Servicios;

namespace ConsumoServiciosSoap.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICountryInfoService _countryInfoService;

    public HomeController(ILogger<HomeController> logger, ICountryInfoService countryInfoService)
    {
        _logger = logger;
        _countryInfoService = countryInfoService;
    }


    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> MostrarCapital()
    {
        string resultado = string.Empty;
        try
        {
            resultado = await _countryInfoService.CapitalCityAsync();
        }
        catch (Exception ex)
        {
            resultado = $"Error al consultar: {ex.Message}";
        }

        ViewBag.Capital = resultado;
        return View("Index");
    }

    public async Task<IActionResult> ListarContinentes()
    {
        var lista = await _countryInfoService.ListOfContinentsByNameAsync();
        ViewBag.ListarContinentes = lista;
        return View("Index");
    }


    public async Task<IActionResult> ConsultarCapital(string codigoPais)
    {
        string resultado = string.Empty;
        try
        {
            // Consumir la lógica del servicio pasando el parámetro
            resultado = await _countryInfoService.ConsultarCapitalPorCodigo(codigoPais);
        }
        catch (Exception ex)
        {
            resultado = $"Error al consultar: {ex.Message}";
        }

        ViewBag.Capital = resultado;
        return View("Index");
    }


    public async Task<IActionResult> ConsultarBanderaPorCodigoPais(string codigoPais)
    {
        string resultado = string.Empty;

        try
        {
            resultado = await _countryInfoService.ConsultarBanderaPorCodigoPais(codigoPais);


        }catch(Exception ex)
        {
            resultado = $"Error al comunicarse con la funcion de mostrar bandera {ex.Message}";
        }

        ViewBag.Bandera = resultado;
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
