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
    public class CitaController : ControllerBase
    {
        private readonly daoCitaWSAsync _dao;
        public CitaController(daoCitaWSAsync dao)
        {
            _dao = dao;
        }

        // GET api/cita
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<CitaViewModel>>> ObtenerCitas()
        {
            try
            {
                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                Console.WriteLine($"Petición GET citas hecha por: {usuarioLogueado}");

                // Llamada al DAO para obtener los citas
                var citas = await _dao.ObtenerCitasAsync();

                // Verificación de resultados
                return Ok(new ApiResponse<List<CitaViewModel>>(200, $"Citas obtenidos correctamente.  Petición hecha por: {usuarioLogueado}", citas));
            }
            catch (Exception e)
            {
                // Manejo de errores
                return StatusCode(500, new ApiResponse<List<CitaViewModel>>(500, $"Error: {e.Message}"));
            }

        }

        // GET api/cita/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCitaPorId(int id)
        {
            try
            {
                var cita = await _dao.ObtenerCitaPorIdAsync(id);
                if (cita == null) return NotFound(new ApiResponse<CitaViewModel>(404, "Cita no encontrada"));
                return Ok(new ApiResponse<CitaViewModel>(200, "Cita encontrada", cita));
            }
            catch (Exception e)
            {   
                // Manejo de errores
                return StatusCode(500, new ApiResponse<CitaViewModel>(500, $"Error: {e.Message}"));
            }

        }

        // POST api/cita
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CrearCita([FromBody] CitaViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<CitaViewModel>(400, "Modelo inválido"));
                }

                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                Console.WriteLine($"Petición POST citas hecha por: {usuarioLogueado}");

                // Llamada al DAO para crear un nuevo paciente
                await _dao.CrearCitaAsync(model);

                // Verificación de resultados
                // Usamos CreatedAtAction para devolver el URI del nuevo recurso
                // Así verificamos que la cita se ha creado correctamente
                return CreatedAtAction(nameof(ObtenerCitaPorId), new { id = model.IdCita },
                    new ApiResponse<CitaViewModel>(201, $"Cita creada correctamente. Petición hecha por: {usuarioLogueado}", model));

            }
            catch (Exception e)
            {
                // Manejo de errores
                return StatusCode(500, new ApiResponse<CitaViewModel>(500, $"Error: {e.Message}"));
            }
        }

        // PUT api/cita/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCita(int id, [FromBody] CitaViewModel model)
        {

            var cita = await _dao.ObtenerCitaPorIdAsync(id);
            if (cita == null)
            {
                return NotFound(new { mensaje = "Cita no encontrada." });
            }

            // Actualiza solo los campos necesarios
            cita.Fk_IdPaciente = model.Fk_IdPaciente;
            cita.Fk_IdMedico = model.Fk_IdMedico;
            cita.Fecha = model.Fecha;
            cita.Hora = model.Hora;

            await _dao.ActualizarCitaAsync(cita);

            return Ok(new { mensaje = "Cita actualizada correctamente.", data = cita });
        }

        // DELETE api/cita/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCita(int id)
        {
            try
            {
                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición DELETE citas hecha por: {usuarioLogueado}");

                // Llamada al DAO para eliminar el paciente
                await _dao.EliminarCitaAsync(id);

                // Respondemos con un mensaje de éxito
                return Ok(new ApiResponse<CitaViewModel>(200, $"Cita eliminada correctamente. Petición hecha por: {usuarioLogueado}"));
            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<CitaViewModel>(404, nfex.Message));
            }

            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<CitaViewModel>(500, $"Error: {e.Message}"));
            }


        }
    }
}
