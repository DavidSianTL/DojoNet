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
    public class RolesController : ControllerBase
    {
        private readonly daoRoles _daoRoles;
        private readonly IBitacoraService _bitacoraService;

        // Constructor que recibe el daoSucursales y IBitacoraService para inyección de dependencias
        public RolesController(daoRoles daoRoles, IBitacoraService bitacoraService)
        {
            _daoRoles = daoRoles;
            _bitacoraService = bitacoraService;
        }

        // Método para obtener la lista de roles
        [HttpGet]
        public ActionResult<List<RolesViewModel>> ObtenerRoles()
        {
            try
            {
                // Obtenemos la lista de roles del daoRoles
                var roles = _daoRoles.ObtenerRoles();

                // Guardamos la acción en la bitácora
                _bitacoraService.RegistrarAccion("Obtener Roles", $"Número de roles obtenidos: {roles.ToArray()}");

                return Ok(roles);
            }
            catch (Exception e)
            {
                // Manejo de excepciones
                return NotFound($"Error al obtener los roles: {e.Message}");
            }
        }
        
    }
}
