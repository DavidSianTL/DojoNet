using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trabajo_APIRest.DAO;
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
                var medicos = await _dao.ObtenerMedicosAsync();

                // Verificación de resultados
                return Ok(new ApiResponse<List<MedicoViewModel>>(200, $"Medicos obtenidos correctamente.  Petición hecha por: {usuarioLogueado}", medicos));
            }
            catch (Exception e)
            {
                // Manejo de errores
                Console.WriteLine("Error al obtener los médicos: " + e.Message);
                return StatusCode(500, new ApiResponse<List<MedicoViewModel>>(500, "Error: Error al obtener los médicos"));
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
                Console.WriteLine("Error al obtener el médico: " + e.Message);
                return StatusCode(500, new ApiResponse<MedicoViewModel>(500, "Error: Error al obtener el médico"));
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
                Console.WriteLine("Error al crear el médico: " + e.Message);
                return StatusCode(500, new ApiResponse<MedicoViewModel>(500, "Error: Error al crear el médico"));
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
                Console.WriteLine("Error al actualizar el médico: " + nfex.Message);
                return NotFound(new ApiResponse<MedicoViewModel>(404, "Médico no encontrado"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al actualizar el médico: " + e.Message);
                return StatusCode(500, new ApiResponse<MedicoViewModel>(500, "Error: Error al actualizar el médico"));
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

                // Verificar si el médico tiene citas futuras
                var medico = await _dao.ObtenerMedicoPorIdAsync(id);
                if (medico == null)
                    return NotFound(new ApiResponse<MedicoViewModel>(404, "Médico no encontrado"));

                if(medico.Especialidades != null && medico.Especialidades.Any())
                {
                    return BadRequest(new ApiResponse<MedicoViewModel>(400, "No se puede eliminar el médico porque tiene especialidades asignadas"));
                }

                if (medico.Citas != null && medico.Citas.Any())
                {
                    return BadRequest(new ApiResponse<MedicoViewModel>(400, "No se puede eliminar el médico porque tiene citas programadas"));
                }

                // Llamada al DAO para eliminar el médico
                // Las relaciones en MedicoEspecialidad se eliminarán en cascada
                await _dao.EliminarMedicoAsync(id);

                // Respondemos con un mensaje de éxito
                return Ok(new ApiResponse<MedicoViewModel>(200, $"Médico eliminado correctamente. Petición hecha por: {usuarioLogueado}"));
            }
            catch (NotFoundException nfex)
            {
                Console.WriteLine("Error al eliminar el médico: " + nfex.Message);
                return NotFound(new ApiResponse<MedicoViewModel>(404, "Médico no encontrado"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al eliminar el médico: " + e.Message);
                return StatusCode(500, new ApiResponse<MedicoViewModel>(500, "Error: Error al eliminar el médico, verifica que no tenga especialidades o citas asociadas."));
            }
        }

    }
}
