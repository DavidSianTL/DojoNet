using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiClinicaMedica.Models;
using ApiClinicaMedica.Daos;
using ApiClinicaMedica.Models.Responses;

namespace ApiClinicaMedica.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v4/[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly CitaDao _dao;

        public CitasController(CitaDao dao)
        {
            _dao = dao;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var citas = await _dao.ListarAsync();
            return Ok(new ApiResponse<List<Cita>>(200, "Listado de citas", citas));
        }

        [HttpPost]
        public async Task<IActionResult> Post(Cita c)
        {
            await _dao.InsertarAsync(c);
            return Ok(new ApiResponse<Cita>(200, "Cita creada correctamente", c));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Cita c)
        {
            if (id != c.IdCita)
                return BadRequest(new ApiResponse<string>(400, "El ID de la URL no coincide con el del cuerpo"));

            var actualizado = await _dao.ActualizarAsync(c);
            if (!actualizado)
                return NotFound(new ApiResponse<string>(404, "Cita no encontrada"));

            return Ok(new ApiResponse<string>(200, "Cita actualizada correctamente"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _dao.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new ApiResponse<string>(404, "Cita no encontrada"));

            return Ok(new ApiResponse<string>(200, "Cita eliminada correctamente"));
        }
    }
}
