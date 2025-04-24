using EjemploConsumoServicioSOAP.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EjemploConsumoServicioSOAP.Controllers
{
    public class BanderaController : Controller
    {
        private readonly ICountryInfo _countryInfo;

        public BanderaController(ICountryInfo countryInfo)
        {
            _countryInfo = countryInfo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Consultar(string codigoPais)
        {
            try
            {
                var urlBandera = await _countryInfo.ObtenerBanderaPorCodigo(codigoPais);
                ViewBag.CodigoPais = codigoPais.ToUpper();
                ViewBag.UrlBandera = urlBandera;
                return View("Mostrar");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
    }
}