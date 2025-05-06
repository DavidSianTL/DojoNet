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

    public IActionResult IndexL()
    {
        return View("Views/Lenguaje/IndexL.cshtml");
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

    //Listar los lenguajes por medio del nombre
    public async Task<IActionResult> ListarLenguajes()
    {
        var lista = await _countryInfoService.ListOfLanguagesByName();
        ViewBag.ListarLenguajes = lista;
        return View("Views/Lenguaje/IndexL.cshtml");
    }

    //Listar los lenguajes por medio del código ISo del lenguaje
    public async Task<IActionResult> ListarLenguajesPorCodigo()
    {
        var lista = await _countryInfoService.ListOfLanguagesByCode();
        ViewBag.ListarLenguajesPorCodigo = lista;
        return View("Views/Lenguaje/IndexL.cshtml");
    }

    //Busca un lenguaje por medio del código Iso
    public async Task<IActionResult> ConsultarLenguaje(string codigoLenguaje)
    {
        string resultado = string.Empty;
        try
        {
            // Consumir la lógica del servicio pasando el parámetro
            resultado = await _countryInfoService.ConsultarLenguajePorCodigo(codigoLenguaje);
        }
        catch (Exception ex)
        {
            resultado = $"Error al consultar: {ex.Message}";
        }

        ViewBag.ConsultarLenguajeC = resultado;
        return View("Views/Lenguaje/IndexL.cshtml");
    }

    //Busca un lenguaje por medio del código Iso
    public async Task<IActionResult> ConsultarLenguajeNombre(string nombreLenguaje)
    {
        string resultado = string.Empty;
        try
        {
            // Consumir la lógica del servicio pasando el parámetro
            resultado = await _countryInfoService.ConsultarLenguajePorNombre(nombreLenguaje);
        }
        catch (Exception ex)
        {
            resultado = $"Error al consultar: {ex.Message}";
        }

        ViewBag.ConsultarLenguajeN = resultado;
        return View("Views/Lenguaje/IndexL.cshtml");
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
