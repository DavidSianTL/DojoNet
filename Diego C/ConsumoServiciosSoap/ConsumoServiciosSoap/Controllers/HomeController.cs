using ConsumoServiciosSoap.Models;
using ConsumoServiciosSoap.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

  /*  public async Task<IActionResult> MostrarCapital()
    {
        string resultado = await _countryInfoService.CapitalCityAsync();
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
        string resultado = await _countryInfoService.ConsultarCapitalPorCodigo(codigoPais);
        ViewBag.Capital = resultado;
        return View("Index");
    }

    public async Task<IActionResult> ListarPaisesPorCodigo()
    {
        var lista = await _countryInfoService.ListOfCountryNamesByCode();
        ViewBag.PaisesPorCodigo = lista;
        return View("Index");
    }

    public async Task<IActionResult> ListarMonedas()
    {
        var lista = await _countryInfoService.ListOfCurrenciesByCode();
        ViewBag.Monedas = lista;
        return View("Index");
    }

    public async Task<IActionResult> MostrarInfoCompleta(string codigoPais)
    {
        var info = await _countryInfoService.FullCountryInfo(codigoPais);
        ViewBag.InfoCompleta = info;
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
    */

    public async Task<IActionResult> ListarIdiomas()
    {
        var lista = await _countryInfoService.ListOfLanguagesByName();
        ViewBag.Idiomas = lista;
        return View("Index");
    }

    public async Task<IActionResult> ListarIdiomasPorCodigo()
    {
        var lista = await _countryInfoService.ListOfLanguagesByCode();
        ViewBag.IdiomasPorCodigo = lista;
        return View("Index");
    }
    [HttpPost]
    public async Task<IActionResult> MostrarNombreIdioma(string codigoIdioma)
    {
        if (string.IsNullOrEmpty(codigoIdioma))
        {
            ViewBag.Mensaje = "Por favor, ingrese un código de idioma.";
            return View("Index");
        }

        var nombreIdioma = await _countryInfoService.ObtenerNombreDeIdioma(codigoIdioma);
        ViewBag.NombreIdioma = nombreIdioma;
        return View("Index");
    }

    public async Task<IActionResult> LanguageISOCode(string idioma)
    {
        var lista = await _countryInfoService.LanguageISOCode(idioma);
        ViewBag.LanguageISOCode = lista;
        return View("Index");
    }


}
