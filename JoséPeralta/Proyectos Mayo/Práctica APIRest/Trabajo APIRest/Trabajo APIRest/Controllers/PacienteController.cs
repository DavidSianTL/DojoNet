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
    public class PacienteController : ControllerBase
    {
        private readonly daoPacienteWSAsync _dao;
        public PacienteController(daoPacienteWSAsync dao)
        {
            _dao = dao;
        }

        // GET api/paciente
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<PacienteViewModel>>> ObtenerPacientes()
        {
            try
            {
                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                Console.WriteLine($"Petición GET pacientes hecha por: {usuarioLogueado}");

                // Llamada al DAO para obtener los pacientes
                var usuarios = await _dao.ObtenerPacientesAsync();

                // Verificaci�n de resultados
                return Ok(new ApiResponse<List<PacienteViewModel>>(200, $"Pacientes obtenidos correctamente.  Petición hecha por: {usuarioLogueado}", usuarios));
            }
            catch (Exception e)
            {
                // Manejo de errores
                return StatusCode(500, new ApiResponse<List<PacienteViewModel>>(500, $"Error: {e.Message}"));
            }

        }

        // GET api/paciente/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPacientePorId(int id)
        {
            try
            {
                var paciente = await _dao.ObtenerPacientePorIdAsync(id);
                if (paciente == null) return NotFound(new ApiResponse<PacienteViewModel>(404, "Paciente no encontrado"));
                return Ok(new ApiResponse<PacienteViewModel>(200, "Paciente encontrado", paciente));
            }
            catch (Exception e)
            {
                // Manejo de errores
                return StatusCode(500, new ApiResponse<PacienteViewModel>(500, $"Error: {e.Message}"));
            }

        }

        // POST api/paciente
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CrearPaciente([FromBody] PacienteViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<PacienteViewModel>(400, "Modelo inválido"));
                }

                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                Console.WriteLine($"Petición POST pacientes hecha por: {usuarioLogueado}");

                // Llamada al DAO para crear un nuevo paciente
                await _dao.CrearPacienteAsync(model);

                // Verificación de resultados
                // Usamos CreatedAtAction para devolver el URI del nuevo recurso
                // Así verificamos que el paciente se ha creado correctamente
                return CreatedAtAction(nameof(ObtenerPacientePorId), new { id = model.IdPaciente },
                    new ApiResponse<PacienteViewModel>(201, $"Paciente creado correctamente. Petición hecha por: {usuarioLogueado}", model));

            }
            catch (Exception e)
            {
                // Manejo de errores
                return StatusCode(500, new ApiResponse<PacienteViewModel>(500, $"Error: {e.Message}"));
            }
        }

        // PUT api/paciente/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPaciente(int id, [FromBody] PacienteViewModel model)
        {

            var paciente = await _dao.ObtenerPacientePorIdAsync(id);
            if (paciente == null)
            {
                return NotFound(new { mensaje = "Paciente no encontrado." });
            }

            // Actualiza solo los campos necesarios
            paciente.Nombre = model.Nombre;
            paciente.Email = model.Email;
            paciente.Telefono = model.Telefono;
            paciente.FechaNacimiento = model.FechaNacimiento;

            await _dao.ActualizarPacienteAsync(paciente);

            return Ok(new { mensaje = "Paciente actualizado correctamente.", data = paciente });
        }

        // DELETE api/paciente/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPaciente(int id)
        {
            try
            {
                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición DELETE pacientes hecha por: {usuarioLogueado}");

                // Llamada al DAO para eliminar el paciente
                await _dao.EliminarPacienteAsync(id);

                // Respondemos con un mensaje de éxito
                return Ok(new ApiResponse<PacienteViewModel>(200, $"Paciente eliminado correctamente. Petición hecha por: {usuarioLogueado}"));
            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<PacienteViewModel>(404, nfex.Message));
            }

            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<PacienteViewModel>(500, $"Error: {e.Message}"));
            }


        }
    }
}
