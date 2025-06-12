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

        public async Task<IActionResult> ConsultarCapital(string codigoPais)
        {
            string resultado = string.Empty;

            try
            {
                resultado = await _countryInfo.ConsultarCapitalPorCodigo(codigoPais);
            }
            catch (Exception ex)
            {
                resultado = $"Error al consultar: {ex.Message}";
            }

            //Retornar resultado a la vista
            ViewBag.Resultado = resultado;
            return View("Index");
        }
    }
}
