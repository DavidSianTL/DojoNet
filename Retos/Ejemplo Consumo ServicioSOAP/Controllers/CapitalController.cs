using EjemploConsumoServicioSOAP.Service;
using Microsoft.AspNetCore.Mvc;

namespace EjemploConsumoServicioSOAP.Controllers
{
    public class CapitalController : Controller
    {
        private readonly ICountryInfo _countryInfo;
        
        public CapitalController(ICountryInfo countryInfo)
        {
            _countryInfo = countryInfo;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Métodos para consultas específicas por código de país
        public async Task<IActionResult> ConsultarCapital(string codigoPais)
        {
            ViewBag.Resultado = await _countryInfo.ConsultarCapitalPorCodigo(codigoPais);
            return View("Index");
        }

        public async Task<IActionResult> ConsultarNombrePais(string codigoPais)
        {
            ViewBag.Resultado = await _countryInfo.ObtenerNombrePais(codigoPais);
            return View("Index");
        }

        public async Task<IActionResult> ConsultarMonedaPais(string codigoPais)
        {
            ViewBag.Resultado = await _countryInfo.ObtenerMonedaPais(codigoPais);
            return View("Index");
        }

        public async Task<IActionResult> ConsultarBanderaPais(string codigoPais)
        {
            var urlBandera = await _countryInfo.ObtenerBanderaPais(codigoPais);
            ViewBag.Resultado = $"<img src='{urlBandera}' alt='Bandera de {codigoPais}' class='img-fluid' style='max-height: 100px;'/>";
            return View("Index");
        }

        // Métodos para listados generales
        public async Task<IActionResult> ListarContinentesNombre()
        {
            ViewBag.Resultado = await _countryInfo.ListarContinentesPorNombre();
            return View("Index");
        }

        public async Task<IActionResult> ListarContinentesCodigo()
        {
            ViewBag.Resultado = await _countryInfo.ListarContinentesPorCodigo();
            return View("Index");
        }

        public async Task<IActionResult> ListarMonedasNombre()
        {
            ViewBag.Resultado = await _countryInfo.ListarMonedasPorNombre();
            return View("Index");
        }

        public async Task<IActionResult> ListarMonedasCodigo()
        {
            ViewBag.Resultado = await _countryInfo.ListarMonedasPorCodigo();
            return View("Index");
        }

        public async Task<IActionResult> ListarPaisesCodigo()
        {
            ViewBag.Resultado = await _countryInfo.ListarPaisesPorCodigo();
            return View("Index");
        }

        public async Task<IActionResult> ListarPaisesNombre()
        {
            ViewBag.Resultado = await _countryInfo.ListarPaisesPorNombre();
            return View("Index");
        }

        public async Task<IActionResult> ListarPaisesAgrupados()
        {
            ViewBag.Resultado = await _countryInfo.ListarPaisesAgrupadosPorContinente();
            return View("Index");
        }

        // Métodos para consultas de idiomas
        public async Task<IActionResult> ConsultarNombreIdioma(string codigoIdioma)
        {
            ViewBag.Resultado = await _countryInfo.ObtenerNombreIdioma(codigoIdioma);
            return View("Index");
        }

        public async Task<IActionResult> ConsultarIdiomasPais(string codigoPais)
        {
            ViewBag.Resultado = await _countryInfo.ObtenerIdiomasPais(codigoPais);
            return View("Index");
        }
    }
}