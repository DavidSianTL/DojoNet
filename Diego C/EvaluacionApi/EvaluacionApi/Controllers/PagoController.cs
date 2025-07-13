using EvaluacionApi.Models;
using EvaluacionApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EvaluacionApi.Controllers
{
    [ApiController]
    [Route("pago")] 
    public class PagoController : Controller
    {
        
        private readonly PagoService _pagoService;

        public PagoController(PagoService pagoService)
        {
            _pagoService = pagoService;
        }

        [HttpPost]
        public IActionResult Crear([FromBody] Pago nuevo)
        {
            var resultado = _pagoService.Registrar(nuevo);

            if (!resultado.Exitoso)
                return BadRequest(new { mensaje = resultado.Mensaje });

            return Ok(resultado.Resultado);
        }
    }
}

