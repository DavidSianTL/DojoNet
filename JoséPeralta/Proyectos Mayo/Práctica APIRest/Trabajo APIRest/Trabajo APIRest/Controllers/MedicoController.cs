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
    public class MedicoController : ControllerBase
    {
        private readonly daoMedicoWSAsync _dao;
        public MedicoController(daoMedicoWSAsync dao)
        {
            _dao = dao;
        }

        // GET api/medico
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<MedicoViewModel>>> ObtenerMedicos()
        {
            try
            {
                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                Console.WriteLine($"Petición GET médicos hecha por: {usuarioLogueado}");

                // Llamada al DAO para obtener los médicos
                var usuarios = await _dao.ObtenerMedicosAsync();

                // Verificación de resultados
                return Ok(new ApiResponse<List<MedicoViewModel>>(200, $"Medicos obtenidos correctamente.  Petición hecha por: {usuarioLogueado}", usuarios));
            }
            catch (Exception e)
            {
                // Manejo de errores
                return StatusCode(500, new ApiResponse<List<MedicoViewModel>>(500, $"Error: {e.Message}"));
            }

        }

        // GET api/medico/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerMedicoPorId(int id)
        {
            try
            {
                var medico = await _dao.ObtenerMedicoPorIdAsync(id);
                if (medico == null) return NotFound(new ApiResponse<MedicoViewModel>(404, "Médico no encontrado"));
                return Ok(new ApiResponse<MedicoViewModel>(200, "Médico encontrado", medico));
            }
            catch (Exception e)
            {
                // Manejo de errores
                return StatusCode(500, new ApiResponse<MedicoViewModel>(500, $"Error: {e.Message}"));
            }

        }

        // POST api/medico
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CrearMedico([FromBody] MedicoViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<MedicoViewModel>(400, "Modelo inválido"));
                }

                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                Console.WriteLine($"Petición POST médicos hecha por: {usuarioLogueado}");

                // Llamada al DAO para crear un nuevo médico
                await _dao.CrearMedicoAsync(model);

                // Verificación de resultados
                // Usamos CreatedAtAction para devolver el URI del nuevo recurso
                // Así verificamos que el médico se ha creado correctamente
                return CreatedAtAction(nameof(ObtenerMedicoPorId), new { id = model.IdMedico },
                    new ApiResponse<MedicoViewModel>(201, $"Médico creado correctamente. Petición hecha por: {usuarioLogueado}", model));

            }
            catch (Exception e)
            {
                // Manejo de errores
                return StatusCode(500, new ApiResponse<MedicoViewModel>(500, $"Error: {e.Message}"));
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarMedico(int id, [FromBody] MedicoViewModel model)
        {

            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, devolvemos un BadRequest
                return BadRequest(new ApiResponse<MedicoViewModel>(400, "Datos de usuario inválidos"));
            }

            try
            {

                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición PUT médicos hecha por: {usuarioLogueado}");

                // Usamos model.IdMedico para asegurarnos de que el ID del modelo coincide con el ID de la URL
                // De este modo ya no es necesario pasar el ID en el cuerpo de la petición
                model.IdMedico = id;

                // Actualizar el médico
                var resultado = await _dao.ActualizarMedicoAsync(model);
                return Ok(new ApiResponse<MedicoViewModel>(200, $"Médico actualizado correctamente. Petición hecha por: {usuarioLogueado}", resultado));


            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<MedicoViewModel>(404, nfex.Message));
            }
            catch
            {
                return StatusCode(500, new { mensaje = "Error: Al actualizar médico" });
            }

        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarMedico(int id)
        {
            try
            {
                // Registro de la petición
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición DELETE médicos hecha por: {usuarioLogueado}");

                // Llamada al DAO para eliminar el médico
                await _dao.EliminarMedicoAsync(id);

                // Respondemos con un mensaje de éxito
                return Ok(new ApiResponse<MedicoViewModel>(200, $"Médico eliminado correctamente. Petición hecha por: {usuarioLogueado}"));
            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<MedicoViewModel>(404, nfex.Message));
            }

            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<MedicoViewModel>(500, $"Error: {e.Message}"));
            }


        }
    }
}
