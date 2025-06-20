using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiClinicaMedica.Data;
using ApiClinicaMedica.Models;

namespace ApiClinicaMedica.Controllers
{
    [ApiController]
    [Route("api/v4/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly ClinicaDbContext _context;
        public PacientesController(ClinicaDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.Pacientes.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            return paciente == null ? NotFound() : Ok(paciente);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Paciente p)
        {
            _context.Pacientes.Add(p);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Paciente creado correctamente", data = p });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Paciente p)
        {
            if (id != p.IdPaciente) return BadRequest();

            _context.Entry(p).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Paciente actualizado" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pacientes.Any(e => e.IdPaciente == id))
                    return NotFound();

                throw;
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _context.Pacientes.FindAsync(id);
            if (p == null) return NotFound();
            _context.Pacientes.Remove(p);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Paciente eliminado" });
        }
    }
}
