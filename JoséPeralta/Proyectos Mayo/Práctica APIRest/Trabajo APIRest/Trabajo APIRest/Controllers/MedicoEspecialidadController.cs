using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trabajo_APIRest.DAO;
using Trabajo_APIRest.Data;
using Trabajo_APIRest.Models;
using UsuariosApi.Models.Responses;

namespace Trabajo_APIRest.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MedicoEspecialidadController : ControllerBase
    {
        private readonly daoMedicoWSAsync _dao;
        private readonly daoMedicoEspecialidadWSAsync _daoMedicoEspecialidad;

        public MedicoEspecialidadController(daoMedicoWSAsync dao, daoMedicoEspecialidadWSAsync daoMedicoEspecialidad)
        {
            _dao = dao;
            _daoMedicoEspecialidad = daoMedicoEspecialidad;
        }

        // GET api/medicosespecialidad
        [HttpGet]
        public async Task<IActionResult> ObtenerRelaciones(){
            try{
                var relaciones = await _daoMedicoEspecialidad.ObtenerMedicoEspecialidadAsync();
                return Ok(new ApiResponse<List<MedicoEspecialidad>>(200, "Relaciones obtenidas correctamente", relaciones));
            }
            catch (Exception e){
                Console.WriteLine($"Error al obtener las relaciones: {e.Message}");
                return StatusCode(500, new ApiResponse<List<MedicoEspecialidad>>(500, $"Error: Error al obtener las relaciones"));
            }
        }

        // GET api/medicoespecialidad/medico/1
        [HttpGet("medico/{id}")]
        public async Task<IActionResult> ObtenerEspecialidadesDeMedico(int id)
        {
            try
            {
                var especialidades = await _daoMedicoEspecialidad.ObtenerEspecialidadesPorMedicoAsync(id);
                return Ok(new ApiResponse<List<EspecialidadViewModel>>(200, "Especialidades obtenidas correctamente", especialidades));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<List<EspecialidadViewModel>>(500, $"Error: {e.Message}"));
            }
        }

        // POST api/medicoespecialidad
        [HttpPost]
        public async Task<IActionResult> AgregarEspecialidadAMedico([FromBody] MedicoEspecialidadRequest request)
        {
            try
            {
                var resultado = await _daoMedicoEspecialidad.AgregarEspecialidadAMedicoAsync(
                    request.MedicoId,
                    request.EspecialidadId);

                if (resultado == null)
                    return BadRequest(new ApiResponse<object>(400, "La relación ya existe o los IDs no son válidos"));

                return Ok(new ApiResponse<object>(200, "Relación creada correctamente", new
                {
                    id = resultado.IdMedicoEspecialidad,
                    medico = new
                    {
                        id = resultado.Medico.IdMedico,
                        nombre = resultado.Medico.Nombre
                    },
                    especialidad = new
                    {
                        id = resultado.Especialidad.IdEspecialidad,
                        nombre = resultado.Especialidad.Nombre
                    }
                }));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {e.Message}"));
            }
        }

        // PUT api/medicoespecialidad/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarRelacion(int id, [FromBody] MedicoEspecialidad request)
        {
            try
            {
                var relacionActualizada = await _daoMedicoEspecialidad
                    .ActualizarRelacionMedicoEspecialidadAsync(id, request.EspecialidadId);

                if (relacionActualizada == null)
                    return BadRequest(new ApiResponse<object>(400,
                        "No se pudo actualizar la relación. Verifica que la relación exista y que la nueva especialidad no esté ya asignada."));

                return Ok(new ApiResponse<object>(200, "Relación actualizada correctamente", new
                {
                    id = relacionActualizada.IdMedicoEspecialidad,
                    medico = new
                    {
                        id = relacionActualizada.Medico.IdMedico,
                        nombre = relacionActualizada.Medico.Nombre
                    },
                    especialidad = new
                    {
                        id = relacionActualizada.Especialidad.IdEspecialidad,
                        nombre = relacionActualizada.Especialidad.Nombre
                    }
                }));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al actualizar la relación: {e.Message}");
                return StatusCode(500, new ApiResponse<object>(500, "Error al actualizar la relación"));
            }
        }

        // DELETE api/medicoespecialidad/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarRelacion(int id)
        {
            try
            {
                var resultado = await _daoMedicoEspecialidad.EliminarRelacionAsync(id);
                if (!resultado)
                    return NotFound(new ApiResponse<object>(404, "No se encontró la relación especificada"));

                return Ok(new ApiResponse<object>(200, "Relación eliminada correctamente"));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al eliminar la relación: {e.Message}");
                return StatusCode(500, new ApiResponse<object>(500, "Error al eliminar la relación"));
            }
        }

    }

    // Clase auxiliar para el request
    public class MedicoEspecialidadRequest
    {
        public int MedicoId { get; set; }
        public int EspecialidadId { get; set; }
    }

}