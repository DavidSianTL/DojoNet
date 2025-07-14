using MiBanco.Data.DTOs.ClienteDTOs;
using MiBanco.Data.DTOs.PagoDTOs;
using MiBanco.Models;
using MiBanco.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiBanco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        private readonly IPagoService _pagoService;
        public PagoController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }

        public ActionResult AddCliente([FromBody] PagoRequestDTO pagoRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pagoResponse = _pagoService.AddPago(pagoRequest);

            if (pagoResponse.Codigo != 200) return StatusCode(pagoResponse.Codigo, pagoResponse.Mensaje);
            
            return Ok(pagoResponse);
        }
      
    }
}

