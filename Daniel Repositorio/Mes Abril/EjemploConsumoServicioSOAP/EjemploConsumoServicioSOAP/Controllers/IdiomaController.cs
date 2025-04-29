using CountryInfoService;
using EjemploConsumoServicioSOAP.Service;
using Microsoft.AspNetCore.Mvc;

namespace EjemploConsumoServicioSOAP.Controllers
{
    public class IdiomaController : Controller
    {
        private readonly ICountryInfo _countryInfo;

        public IdiomaController(ICountryInfo countryInfo)
        {
            _countryInfo = countryInfo;
        }

        // Lista de idiomas por nombre
        public async Task<IActionResult> Index()
        {
            var idiomas = await _countryInfo.ObtenerIdiomas();
            return View(idiomas);
        }

        // Lista de idiomas por código
        public async Task<IActionResult> PorCodigo()
        {
            var idiomas = await _countryInfo.ObtenerIdiomasPorCodigo();
            return View(idiomas);
        }

        // Mostrar formulario de búsqueda
        [HttpGet]
        public IActionResult Buscar()
        {
            return View(); 
        }

        // Buscar nombre de idioma por código ISO
        [HttpGet]
        public IActionResult BuscarCodigo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuscarCodigo(string codigoIdioma)
        {
            var resultado = await _countryInfo.ObtenerNombreIdiomaPorCodigo(codigoIdioma);
            ViewBag.Resultado = resultado;
            return View();
        }

        // Buscar código ISO por nombre de idioma
        [HttpPost]
        public async Task<IActionResult> BuscarNombre(string nombreIdioma)
        {
            string resultado = await _countryInfo.ObtenerCodigoIdiomaPorNombre(nombreIdioma);
            ViewBag.ResultadoNombre = resultado;
            return View("Buscar"); 
        }
    }
}
