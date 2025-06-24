using API2.Data.DAOs;
using API2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly CitaDAO _citaDao;

        public CitasController(CitaDAO citaDao)
        {
            _citaDao = citaDao;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cita cita)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != cita.Id)
                return BadRequest(new
                {
                    responseCode = 400,
                    responseMessage = "ID no coincide"
                });

            var updated = await _citaDao.Actualizar(cita);
            if (!updated)
                return NotFound(new
                {
                    responseCode = 404,
                    responseMessage = "Cita no encontrada"
                });

            return Ok(new
            {
                responseCode = 200,
                responseMessage = "Cita actualizada correctamente"
            });
        }
    }
}

