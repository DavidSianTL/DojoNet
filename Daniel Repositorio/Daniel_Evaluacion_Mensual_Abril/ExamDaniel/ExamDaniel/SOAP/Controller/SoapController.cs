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

       
        [HttpGet]
        //Detalle de informacion de pais
        public async Task<IActionResult> Detalle()
        {
          
            var paises = await _soapService.ObtenerPaisesPorCodigoAsync();

           
            if (paises == null || paises.Length == 0)
            {
                // Si no hay países, manejar el caso (puedes agregar un mensaje de error)
                ViewBag.CodigosPaises = new List<SelectListItem>();
            }
            else
            {
                ViewBag.CodigosPaises = paises.Select(p => new SelectListItem
                {
                    Value = p.sISOCode, 
                    Text = p.sName 
                }).ToList();
            }

            return View();
        }

        [HttpPost]
        // obtener informacion completa de pais
        public async Task<IActionResult> Detalle(string codigo)
        {
            var info = await _soapService.ObtenerInformacionCompletaAsync(codigo);

          
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
