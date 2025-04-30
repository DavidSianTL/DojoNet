using Microsoft.AspNetCore.Mvc;
using ExamDaniel.Servicios;
using CountryServiceReference;

namespace ExamDaniel.Controllers
{
    public class SoapController : Controller
    {
        private readonly CountryInfoService _soapService;

        public SoapController()
        {
            _soapService = new CountryInfoService(); // Sin inyección por simplicidad
        }
        public IActionResult Menu()
        {
            return View();
        }


        public async Task<IActionResult> Index()
        {
            var paises = await _soapService.ObtenerPaisesPorNombreAsync();
            return View(paises);
        }

        public async Task<IActionResult> PorCodigo()
        {
            var paises = await _soapService.ObtenerPaisesPorCodigoAsync();
            return View(paises);
        }


        [HttpGet]
        public IActionResult Detalle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Detalle(string codigo)
        {
            var info = await _soapService.ObtenerInformacionCompletaAsync(codigo);
            return View(info);
        }

    }
}
