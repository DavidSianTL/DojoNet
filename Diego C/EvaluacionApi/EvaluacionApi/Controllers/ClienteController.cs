using EvaluacionApi.Models;
using EvaluacionApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EvaluacionApi.Controllers
{
    [ApiController]
    [Route("cliente")]
    public class ClienteController : Controller
    {
        private readonly ClienteService _servicio;

        public ClienteController(ClienteService servicio)
        {
            _servicio = servicio;
        }

        [HttpGet("{dpi}")]
        public IActionResult ObtenerPorDpi(string dpi)
        {
            var cliente = _servicio.ObtenerPorDpi(dpi);
            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            return Ok(cliente);
        }


        [HttpPost]
        public IActionResult Crear([FromBody] Cliente nuevo)
        {
            var creado = _servicio.Crear(nuevo);

            if (creado == null)
                return BadRequest(new { mensaje = "Ya existe un cliente con ese DPI." });

            return CreatedAtAction(nameof(ObtenerPorDpi), new { dpi = creado.Dpi }, creado);
        }

    }
}