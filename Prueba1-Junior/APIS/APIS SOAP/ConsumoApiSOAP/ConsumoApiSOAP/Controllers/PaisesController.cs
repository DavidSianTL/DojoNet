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
    }

}
