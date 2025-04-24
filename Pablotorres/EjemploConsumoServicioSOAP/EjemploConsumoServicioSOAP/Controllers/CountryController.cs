using EjemploConsumoServicioSOAP.Models;
using EjemploConsumoServicioSOAP.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EjemploConsumoServicioSOAP.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryInfo _service;

        public CountryController(ICountryInfo service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

       
        public async Task<ActionResult> ObtenerBandera(string codigoPais)
        {
            string result = string.Empty;
            try
            {

                result = await _service.ObtenerBanderaPorCodigo(codigoPais);
            }

            catch  (Exception ex) {
                result = $"Error al consultar la Bandera del Pais:{ ex.Message }"  ;

            }
            ViewBag.Result = result;

            return View("Index");
          
                
         
           
        }
    }
}

