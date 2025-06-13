using Microsoft.AspNetCore.Mvc;
using ConsumoApiSOAP.Services;

namespace ConsumoApiSOAP.Controllers
{
    public class PaisesController : Controller
    {
        private readonly ICountryInfoService _countryInfoService;

        public PaisesController(ICountryInfoService countryInfoService)
        {
            _countryInfoService = countryInfoService;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PaisPorCodigo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaisPorCodigo(string codigoISO)
        {
            string pais = string.Empty;
            
            pais =  await _countryInfoService.paisPorCodigoAsync(codigoISO);

            ViewBag.Pais = pais;
            return View();
        }



        [HttpGet]
        public IActionResult CodigoPorPais()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> CodigoPorPais(string nombrePais)
        {
            string codigoPais = string.Empty;

            codigoPais = await _countryInfoService.CodigoPorPaisAsync(nombrePais);

            ViewBag.CodigoPais = codigoPais;


            return View();
        }


        [HttpGet] 
        public async Task<IActionResult> ListadoDePaises()
        {
            List<string> paises = new List<string>();

            paises = await _countryInfoService.ListadoDePaisesAsync();

            return View(paises);
        }
    }

}
