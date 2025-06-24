using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Trabajo_APIRest.Data;
using Trabajo_APIRest.Models;
using UsuariosApi.Exceptions;
using UsuariosApi.Models.Responses;

namespace Trabajo_APIRest.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadController : ControllerBase
    {
        private readonly daoEspecialidadWSASync _dao;
        public EspecialidadController(daoEspecialidadWSASync dao)
        {
            _dao = dao;
        }

        // GET api/especialidad
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<EspecialidadViewModel>>> ObtenerEspecialidades()
        {
            try
            {
                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                Console.WriteLine($"Petición GET especialidades hecha por: {usuarioLogueado}");

                // Llamada al DAO para obtener los especialidades
                var especialidades = await _dao.ObtenerEspecialidadesAsync();

                // Verificación de resultados
                return Ok(new ApiResponse<List<EspecialidadViewModel>>(200, $"Especialidades obtenidos correctamente.  Petición hecha por: {usuarioLogueado}", especialidades));
            }
            catch (Exception e)
            {
                // Manejo de errores
                return StatusCode(500, new ApiResponse<List<EspecialidadViewModel>>(500, $"Error: {e.Message}"));
            }

        }

        // GET api/especialidad/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerEspecialidadPorId(int id)
        {
            try
            {
                var especialidad = await _dao.ObtenerEspecialidadPorIdAsync(id);
                if (especialidad == null) return NotFound(new ApiResponse<EspecialidadViewModel>(404, "Especialidad no encontrada"));
                return Ok(new ApiResponse<EspecialidadViewModel>(200, "Especialidad encontrada", especialidad));
            }
            catch (Exception e)
            {   
                // Manejo de errores
                return StatusCode(500, new ApiResponse<EspecialidadViewModel>(500, $"Error: {e.Message}"));
            }

        }

        // POST api/especialidad
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CrearEspecialidad([FromBody] EspecialidadViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<EspecialidadViewModel>(400, "Modelo inválido"));
                }

                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                Console.WriteLine($"Petición POST especialidades hecha por: {usuarioLogueado}");

                // Llamada al DAO para crear un nuevo paciente
                await _dao.CrearEspecialidadAsync(model);

                // Verificación de resultados
                // Usamos CreatedAtAction para devolver el URI del nuevo recurso
                // Así verificamos que el paciente se ha creado correctamente
                return CreatedAtAction(nameof(ObtenerEspecialidadPorId), new { id = model.IdEspecialidad },
                    new ApiResponse<EspecialidadViewModel>(201, $"Especialidad creada correctamente. Petición hecha por: {usuarioLogueado}", model));

            }
            catch (Exception e)
            {
                // Manejo de errores
                return StatusCode(500, new ApiResponse<EspecialidadViewModel>(500, $"Error: {e.Message}"));
            }
        }

        // PUT api/especialidad/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEspecialidad(int id, [FromBody] EspecialidadViewModel model)
        {

            var especialidad = await _dao.ObtenerEspecialidadPorIdAsync(id);
            if (especialidad == null)
            {
                return NotFound(new { mensaje = "Especialidad no encontrada." });
            }

            // Actualiza solo los campos necesarios
            especialidad.Nombre = model.Nombre;

            await _dao.ActualizarEspecialidadAsync(especialidad);

            return Ok(new { mensaje = "Especialidad actualizada correctamente.", data = especialidad });
        }

        // DELETE api/especialidad/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEspecialidad(int id)
        {
            try
            {
                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición DELETE especialidades hecha por: {usuarioLogueado}");

                // Llamada al DAO para eliminar el paciente
                await _dao.EliminarEspecialidadAsync(id);

                // Respondemos con un mensaje de éxito
                return Ok(new ApiResponse<EspecialidadViewModel>(200, $"Especialidad eliminada correctamente. Petición hecha por: {usuarioLogueado}"));
            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<EspecialidadViewModel>(404, nfex.Message));
            }

            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<EspecialidadViewModel>(500, $"Error: {e.Message}"));
            }


        }
    }
}
