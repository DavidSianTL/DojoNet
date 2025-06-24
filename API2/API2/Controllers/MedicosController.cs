using API2.Models;
using API2.Data.DAOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq; // Necesario para Select y LINQ

namespace API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly MedicoDAO _medicoDao;

        public MedicosController(MedicoDAO medicoDao)
        {
            _medicoDao = medicoDao;
        }

        // POST: api/Medicos
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PostMedico([FromBody] Medico medico)
        {
            if (!ModelState.IsValid)
            {
                // Extraer solo los errores limpios (campo : lista de errores)
                var errores = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key, // propiedad que falló ("Nombre", "Especialidad", etc.)
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray() // lista de mensajes de error
                    );

                return BadRequest(new
                {
                    responseCode = 400,
                    responseMessage = "Datos del modelo no válidos",
                    errors = errores
                });
            }

            await _medicoDao.Crear(medico);

            return CreatedAtAction("PostMedico", new { id = medico.Id }, new
            {
                responseCode = 201,
                responseMessage = "Médico creado correctamente",
                data = medico
            });
        }
    }
}
