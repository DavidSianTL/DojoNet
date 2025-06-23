using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicaApi.Data;
using ClinicaApi.Models;
using ClinicaApi.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace ClinicaApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MedicoEspecialidadController : ControllerBase
    {
        private readonly ClinicaContext _context;

        public MedicoEspecialidadController(ClinicaContext context)
        {
            _context = context;
        }

     
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Asignar([FromBody] MedicoEspecialidad data)
        {
            var medicoExiste = await _context.Medicos.AnyAsync(m => m.Id == data.MedicoId);
            var especialidadExiste = await _context.Especialidades.AnyAsync(e => e.Id == data.EspecialidadId);

            if (!medicoExiste || !especialidadExiste)
            {
                return BadRequest(new ApiResponse<object>(400, "El médico o la especialidad no existe."));
            }

            var yaExiste = await _context.MedicoEspecialidades
                .AnyAsync(me => me.MedicoId == data.MedicoId && me.EspecialidadId == data.EspecialidadId);

            if (yaExiste)
            {
                return BadRequest(new ApiResponse<object>(400, "Esta especialidad ya está asignada al médico."));
            }

            _context.MedicoEspecialidades.Add(data);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>(200, "Especialidad asignada correctamente al médico."));
        }

      
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var relaciones = await _context.MedicoEspecialidades
                .Include(me => me.Medico)
                .Include(me => me.Especialidad)
                .Select(me => new
                {
                    me.MedicoId,
                    MedicoNombre = me.Medico.Nombre,
                    me.EspecialidadId,
                    EspecialidadNombre = me.Especialidad.Nombre
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>(200, "Relaciones obtenidas correctamente", relaciones));
        }

       
       
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Remover([FromQuery] int medicoId, [FromQuery] int especialidadId)
        {
            var relacion = await _context.MedicoEspecialidades
                .FirstOrDefaultAsync(me => me.MedicoId == medicoId && me.EspecialidadId == especialidadId);

            if (relacion == null)
            {
                return NotFound(new ApiResponse<object>(404, "Relación médico-especialidad no encontrada."));
            }

            _context.MedicoEspecialidades.Remove(relacion);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>(200, "Relación eliminada correctamente."));
        }
    }
}
