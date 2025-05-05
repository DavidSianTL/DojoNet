using System.Diagnostics;
using CountryInfoService;
using EjemploApiSoap.Models;
using EjemploApiSoap.Services;
using Microsoft.AspNetCore.Mvc;

namespace EjemploApiSoap.Controllers
{
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

        // Obtener la capital de un País
        public async Task<IActionResult> BuscarCapital(string country)
        {

            // Validar que el país no sea nulo
            if (string.IsNullOrEmpty(country))
            {
                ModelState.AddModelError("country", "El país no puede ser nulo.");
                return View();
            }

            string resultado = string.Empty;

            try
            {
                resultado = await _countryInfo.CountryInfoService(country);

                return View();

            }
            catch (Exception e)
            {
                // Manejo de excepciones
                System.IO.File.WriteAllText("log.txt", e.ToString());
                ModelState.AddModelError("country", "Error al consultar la Capital: " + e.Message);
                return View();
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
}
