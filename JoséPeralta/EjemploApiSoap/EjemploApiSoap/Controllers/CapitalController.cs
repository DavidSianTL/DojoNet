using EjemploApiSoap.Services;
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
            catch (Exception e)
            {
                resultado = $"Error al consultar: {e.Message}";
            }

            //Retornar resultado a la vista
            ViewBag.Resultado = resultado;
            return View("Index");
        }
    }
}
