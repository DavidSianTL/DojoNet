using ApiClinicaMedica.Data;
using ApiClinicaMedica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/v4/[controller]")]
public class CitasController : ControllerBase
{
    private readonly ClinicaDbContext _context;
    public CitasController(ClinicaDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await _context.Citas
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
            .ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Post(Cita c)
    {
        _context.Citas.Add(c);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Cita creada correctamente", data = c });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Cita c)
    {
        if (id != c.IdCita) return BadRequest();

        _context.Entry(c).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cita actualizada" });
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Citas.Any(e => e.IdCita == id))
                return NotFound();

            throw;
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cita = await _context.Citas.FindAsync(id);
        if (cita == null) return NotFound();
        _context.Citas.Remove(cita);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Cita eliminada" });
    }
}
