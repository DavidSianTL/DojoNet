using Microsoft.AspNetCore.Mvc;
using ExamDaniel.Servicios;
using CountryServiceReference;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExamDaniel.Controllers
{
    public class SoapController : Controller
    {
        private readonly CountryInfoService _soapService;

        public SoapController()
        {
            _soapService = new CountryInfoService(); 
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

        // Acción GET para mostrar el formulario de detalle con el dropdown
        [HttpGet]
        public async Task<IActionResult> Detalle()
        {
            // Obtener la lista de países para el dropdown
            var paises = await _soapService.ObtenerPaisesPorCodigoAsync();

            // Verificar si la lista de países tiene elementos
            if (paises == null || paises.Length == 0)
            {
                // Si no hay países, manejar el caso (puedes agregar un mensaje de error)
                ViewBag.CodigosPaises = new List<SelectListItem>();
            }
            else
            {
                ViewBag.CodigosPaises = paises.Select(p => new SelectListItem
                {
                    Value = p.sISOCode, // Código del país
                    Text = p.sName // Nombre del país
                }).ToList();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Detalle(string codigo)
        {
            // Obtener la información completa del país
            var info = await _soapService.ObtenerInformacionCompletaAsync(codigo);

            // Obtener la lista de países nuevamente para mostrarla en el formulario
            var paises = await _soapService.ObtenerPaisesPorCodigoAsync();
            ViewBag.CodigosPaises = paises.Select(p => new SelectListItem
            {
                Value = p.sISOCode,
                Text = p.sName
            }).ToList();

            return View(info);
        }
    }
}
