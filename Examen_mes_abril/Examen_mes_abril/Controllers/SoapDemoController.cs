using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Examen_mes_abril.Models;
using SoapDemoService;
using Examen_mes_abril.Services;

namespace Examen_mes_abril.Controllers
{
    public class SoapDemoController : Controller
    {
        private readonly ILogger<SoapDemoController> _logger;
        private readonly ISoapDemoService _soapDemoService;

        public SoapDemoController(ILogger<SoapDemoController> logger, ISoapDemoService soapDemoService)
        {
            _logger = logger;
            _soapDemoService = soapDemoService;
        }

        public IActionResult Index()
        {
            BitacoraService.RegistrarEvento("Operacion SOAP", $"Se accedió a la vista Index correctamente");
            return View();
        }

        // Acción para consultar personal por ID
        [HttpPost]
        public async Task<IActionResult> ConsultarPersona(string id)
        {
            string resultado;

            try
            {
                resultado = await _soapDemoService.ConsultarPersonaPorId(id);
                BitacoraService.RegistrarEvento("Operacion SOAP", $"Consulta de persona por ID realizada correctamente:{id}");
            }
            catch (Exception ex)
            {
                resultado = $"Error al consultar persona: {ex.Message}";
                BitacoraService.RegistrarEvento("ERROR:", ex.Message);
            }

            ViewBag.ResultadoConsulta = resultado;

            return View("Index");
        }

        // Acción para consultar personal por medio del nombre
        [HttpPost]
        public async Task<IActionResult> ConsultarPersonasPorNombre(string nombre)
        {
            List<string> resultados;

            try
            {
                resultados = await _soapDemoService.ConsultarPersonasPorNombre(nombre);
                BitacoraService.RegistrarEvento("Operacion SOAP", $"Consulta de persona por nombre realizada correctamente:{nombre}");
            }
            catch (Exception ex)
            {
                resultados = new List<string> { $"Error al consultar persona: {ex.Message}" };
                BitacoraService.RegistrarEvento("ERROR:", ex.Message);
            }

            ViewBag.Resultados = resultados;
            return View("Index");
        }

    }
}
