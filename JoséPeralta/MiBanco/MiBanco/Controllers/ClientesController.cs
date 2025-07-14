using MiBanco.Data;
using MiBanco.Models;
using MiBanco.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MiBanco.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly daoClientes _daoClientes;
        private readonly IBitacoraService _bitacoraService;

        // Constructor que recibe el daoClientes y IBitacoraService para inyección de dependencias
        public ClientesController(daoClientes clientes, IBitacoraService bitacoraService)
        {
            _daoClientes = clientes;
            _bitacoraService = bitacoraService;
        }

        // GET: /clientes
        [HttpGet]
        public ActionResult<List<ClientesViewModel>> ObtenerClientes()
        {
            // Obtenemos la lista de clientes
            var clientes = _daoClientes.ObtenerClientes();

            // Guardamos la acción en la bitácora
            _bitacoraService.RegistrarAccion("Obtener Clientes", $"Clientes: {clientes.ToArray()}");


            // Mandamos la lista de clientes como respuesta
            return Ok(clientes);
        }

        // GET: /clientes/{dpi}
        [HttpGet("{dpi}")]
        public ActionResult<List<ClientesViewModel>> ObtenerCliente(string dpi)
        {
            // Obtenemos el cliente por su DPI
            var cliente = _daoClientes.ObtenerClientePorDPI(dpi);

            // Si el cliente no existe, retornamos un error 404
            if (cliente == null)
            {
                return NotFound($"Cliente con DPI: {dpi} no encontrado.");
            }

            // Guardamos la acción en la bitácora
            _bitacoraService.RegistrarAccion("Obtener Cliente", $"Cliente: Id = {cliente.Id}, DPI = {cliente.DPI}, Nombre = {cliente.NombreCompleto}");

            // Retornamos el cliente encontrado
            return Ok(cliente);
        }

        // POST: /clientes
        [HttpPost]
        public ActionResult CrearCliente(ClientesViewModel cliente)
        {
            // Vemos que el modelo sea válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Vamos a buscar si el cliente ya existe por medio del DPI
            var clienteExistente = _daoClientes.ObtenerClientePorDPI(cliente.DPI);
            if (clienteExistente != null)
            {
                return Conflict($"Ya existe un cliente con DPI: {cliente.DPI}.");
            }

            // Agregar el cliente
            _daoClientes.AgregarCliente(cliente);

            // Guardamos la acción en la bitácora
            _bitacoraService.RegistrarAccion("Creación de Cliente", $"Nuevo cliente: {cliente}");

            return CreatedAtAction(nameof(ObtenerCliente), new { dpi = cliente.DPI }, cliente);
        }

    }
}
