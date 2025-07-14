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
    public class SucursalesController : ControllerBase
    {
        private readonly daoSucursales _daoSucursales;
        private readonly IBitacoraService _bitacoraService;

        // Constructor que recibe el daoSucursales y IBitacoraService para inyección de dependencias
        public SucursalesController(daoSucursales daoSucursales, IBitacoraService bitacoraService)
        {
            _daoSucursales = daoSucursales;
            _bitacoraService = bitacoraService;
        }

        // Método para obtener la lista de sucursales
        [HttpGet]
        public ActionResult<List<SucursalViewModel>> ObtenerSucursales()
        {
            try
            {
                // Obtenemos la lista de sucursales del daoSucursales
                var sucursales = _daoSucursales.ObtenerSucursales();

                // Guardamos la acción en la bitácora
                _bitacoraService.RegistrarAccion("Obtener Sucursales", $"Número de sucursales obtenidas: {sucursales.ToArray()}");

                return Ok(sucursales);
            }
            catch (Exception e)
            {
                // Manejo de excepciones
                return NotFound($"Error al obtener las sucursales: {e.Message}");
            }
        }


    }
}
