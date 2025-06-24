using Asp.Versioning;
using ClinicaApi.DAO;
using ClinicaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClinicaApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly daoCitaAsyncEF _daoCita;

        public CitasController(daoCitaAsyncEF daoCita)
        {
            _daoCita = daoCita;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var citas = await _daoCita.GetAllAsync();
            return Ok(new
            {
                responseCode = 200,
                responseMessage = "Citas obtenidas correctamente",
                data = citas
            });
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cita = await _daoCita.GetByIdAsync(id);
            if (cita == null)
                return NotFound(new
                {
                    responseCode = 404,
                    responseMessage = "Cita no encontrada"
                });

            return Ok(new
            {
                responseCode = 200,
                responseMessage = "Cita obtenida correctamente",
                data = cita
            });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cita cita)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _daoCita.CreateAsync(cita);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new
            {
                responseCode = 201,
                responseMessage = "Cita creada correctamente",
                data = created
            });
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cita cita)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != cita.Id)
                return BadRequest(new { responseCode = 400, responseMessage = "ID no coincide" });

            var updated = await _daoCita.UpdateAsync(cita);
            if (!updated)
                return NotFound(new { responseCode = 404, responseMessage = "Cita no encontrada" });

            return Ok(new { responseCode = 200, responseMessage = "Cita actualizada correctamente" });
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _daoCita.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { responseCode = 404, responseMessage = "Cita no encontrada" });

            return Ok(new { responseCode = 200, responseMessage = "Cita eliminada correctamente" });
        }
    }
}
