using ClinicaMedicaAPIREST.Data.DAOs;
using ClinicaMedicaAPIREST.Data.DTO.EspecialidadesDTOs;
using ClinicaMedicaAPIREST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedicaAPIREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        private readonly daoEspecialidades _daoEspecialidade;
        public EspecialidadesController(daoEspecialidades daoEspecialidade)
        {
            _daoEspecialidade = daoEspecialidade;
        }

        [HttpGet]
        [Authorize(Roles = "sysAdmin, medico")]
        public async Task<ActionResult> ObtenerEspecialidades()
        {
            var especialidades = await _daoEspecialidade.GetEspecialidadesAsync();

            if (especialidades == null) return NotFound("No se encontraron especialidades.");

            return Ok(especialidades);
        }

        [HttpPost]
        [Authorize(Roles = "sysAdmin")]
        public async Task<ActionResult> InsertarEspecialidad([FromBody] EspecialidadRequestDTO especialidadRequest)
        {
            if (!ModelState.IsValid) return BadRequest("Datos de especialidad no válidos, intente de nuevo.");
            
            var response = await _daoEspecialidade.AddEspecialidadync(especialidadRequest);

            if (!response) return BadRequest("Error al insertar la especialidad.");
            
            return Ok("Especialidad agregada correctamente");
        }


        [HttpPut]
        [Authorize(Roles = "sysAdmin")]
        public async Task<IActionResult> ActualizarEspecialidad([FromBody] Especialidad especialidad)
        {
            if(!ModelState.IsValid) return BadRequest("Datos de especialidad no válidos, intente de nuevo.");
        
           var response = await _daoEspecialidade.UpdateEspecialidadAsync(especialidad);
           if (!response) return BadRequest("Error al actualizar la especialidad.");

            return Ok("Especialidad actualizada correctamente.");
         
        }

        [HttpDelete]
        [Authorize(Roles = "sysAdmin")]
        public async Task<ActionResult> EliminarEspecialidad([FromBody] int Id)
        {
            if (Id <= 0) return BadRequest("Id de especialidad no válido.");

            var response = await _daoEspecialidade.DeleteEspecialidadAsync(Id);
            if (!response) return BadRequest("Error al eliminar la especialidad.");

            return Ok("Especialidad eliminada correctamente");
        }
    }
}
