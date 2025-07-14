using MiBanco.Data.DAOs;
using MiBanco.Data.DTOs.ClienteDTOs;
using MiBanco.Helpers;
using MiBanco.Models;
using MiBanco.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Frozen;

namespace MiBanco.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClienteController : ControllerBase
	{
		private IClienteService _clienteService;
		private readonly ILogService _logService;
        public ClienteController(IClienteService clienteService,ILogService logService)
        {
			_clienteService = clienteService;
			_logService = logService;
        }
        

        [HttpGet]
		public ActionResult<ClienteResponseDTO> GetCliente([FromBody] string DPI)
		{
			var clienteResponse = _clienteService.GetCliente(DPI);

			return Ok(clienteResponse);
        }

		[HttpPost]
		public ActionResult AddCliente([FromBody] ClienteRequestDTO clienteRequest)
		{
            if (!ModelState.IsValid) return BadRequest(ModelState);
			var response = _clienteService.AddCliente(clienteRequest);

			if (!response.Exito) return StatusCode(response.Codigo, response.Mensaje);

            return Ok(response);
		}

	}
}
