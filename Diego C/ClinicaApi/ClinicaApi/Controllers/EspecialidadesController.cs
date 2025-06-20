using Asp.Versioning;
using ClinicaApi.DAO;
using ClinicaApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClinicaApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly daoEspecialidadAsyncEF _daoEspecialidad;

        public EspecialidadesController(daoEspecialidadAsyncEF daoEspecialidad)
        {
            _daoEspecialidad = daoEspecialidad;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var especialidades = await _daoEspecialidad.GetAllAsync();
            return Ok(new
            {
                responseCode = 200,
                responseMessage = "Especialidades obtenidas correctamente",
                data = especialidades
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var especialidad = await _daoEspecialidad.GetByIdAsync(id);
            if (especialidad == null)
                return NotFound(new
                {
                    responseCode = 404,
                    responseMessage = "Especialidad no encontrada"
                });

            return Ok(new
            {
                responseCode = 200,
                responseMessage = "Especialidad obtenida correctamente",
                data = especialidad
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Especialidad especialidad)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _daoEspecialidad.CreateAsync(especialidad);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new
            {
                responseCode = 201,
                responseMessage = "Especialidad creada correctamente",
                data = created
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Especialidad especialidad)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != especialidad.Id)
                return BadRequest(new { responseCode = 400, responseMessage = "ID no coincide" });

            var updated = await _daoEspecialidad.UpdateAsync(especialidad);
            if (!updated)
                return NotFound(new { responseCode = 404, responseMessage = "Especialidad no encontrada" });

            return Ok(new { responseCode = 200, responseMessage = "Especialidad actualizada correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _daoEspecialidad.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { responseCode = 404, responseMessage = "Especialidad no encontrada" });

            return Ok(new { responseCode = 200, responseMessage = "Especialidad eliminada correctamente" });
        }
    }
}
