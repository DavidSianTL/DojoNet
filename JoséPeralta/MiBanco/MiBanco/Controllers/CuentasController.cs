using MiBanco.Data;
using MiBanco.Models;
using MiBanco.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiBanco.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CuentasController : ControllerBase
    {

        private readonly daoCuentas _daoCuentas;
        private readonly IBitacoraService _bitacoraService;

        // Constructor que recibe el daoCuentas y IBitacoraService para inyección de dependencias
        public CuentasController(daoCuentas daoCuentas, IBitacoraService bitacoraService)
        {
            _daoCuentas = daoCuentas;
            _bitacoraService = bitacoraService;
        }

        // Método para obtener la lista de cuentas
        [HttpGet]
        public ActionResult<List<CuentasViewModel>> ObtenerCuentas()
        {
            try
            {
                // Obtenemos la lista de cuentas del daoCuentas
                var cuentas = _daoCuentas.ObtenerCuentas();
                return Ok(cuentas);
            }
            catch (Exception e)
            {
                // Manejo de excepciones
                return NotFound($"Error al obtener las cuentas: {e.Message}");
            }
        }

    }
}
