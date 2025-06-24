using API2.Data.DAOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        private readonly EspecialidadDAO _especialidadDao;

        public EspecialidadesController(EspecialidadDAO especialidadDao)
        {
            _especialidadDao = especialidadDao;
        }

        // DELETE: api/Especialidades/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEspecialidad(int id)
        {
            var eliminado = await _especialidadDao.Eliminar(id);

            if (!eliminado)
            {
                return NotFound(new
                {
                    responseCode = 404,
                    responseMessage = "Especialidad no encontrada"
                });
            }

            return Ok(new
            {
                responseCode = 200,
                responseMessage = "Especialidad eliminada correctamente"
            });
        }
    }
}
