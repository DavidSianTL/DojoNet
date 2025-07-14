using MiBanco.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiBanco.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BitacoraController : Controller
    {

        private readonly daoBitacora _daoBitacora;

        // Inyección de dependencias para el daoBitacora
        public BitacoraController(daoBitacora daoBitacora)
        {
            _daoBitacora = daoBitacora;
        }

        // GET: /bitacora
        [HttpGet]
        public ActionResult ObtenerBitacora()
        {
            // Obtenemos la lista de bitácoras
            var bitacoras = _daoBitacora.ObtenerBitacoras();
            // Retornamos la lista de bitácoras como respuesta
            return Ok(bitacoras);
        }

    }
}
