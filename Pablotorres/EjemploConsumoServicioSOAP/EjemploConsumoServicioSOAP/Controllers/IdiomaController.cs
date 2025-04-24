using EjemploConsumoServicioSOAP.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CountryInfoService;

namespace EjemploConsumoServicioSOAP.Controllers
{
    public class IdiomaController : Controller
    {
        private readonly ICountryInfo _countryInfo;

        public IdiomaController(ICountryInfo countryInfo)
        {
            _countryInfo = countryInfo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListarPorNombre()
        {
            try
            {
                var idiomas = await _countryInfo.ListarIdiomasPorNombre();
                return View(idiomas);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        public async Task<IActionResult> ListarPorCodigo()
        {
            try
            {
                var idiomas = await _countryInfo.ListarIdiomasPorCodigo();
                return View(idiomas);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        public IActionResult ConsultarNombre()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConsultarNombre(string codigoIdioma)
        {
            try
            {
                var nombreIdioma = await _countryInfo.ObtenerNombreIdiomaPorCodigo(codigoIdioma);
                ViewBag.NombreIdioma = nombreIdioma;
                ViewBag.CodigoIdioma = codigoIdioma.ToUpper();
                return View("MostrarNombre");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        public IActionResult ConsultarCodigo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConsultarCodigo(string nombreIdioma)
        {
            try
            {
                var codigoISO = await _countryInfo.ObtenerCodigoISOPorNombreIdioma(nombreIdioma);
                ViewBag.CodigoISO = codigoISO;
                ViewBag.NombreIdioma = nombreIdioma;
                return View("MostrarCodigo");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
    }
}
