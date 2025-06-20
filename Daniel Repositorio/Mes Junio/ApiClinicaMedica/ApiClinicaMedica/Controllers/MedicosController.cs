using ApiClinicaMedica.Data;
using ApiClinicaMedica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/v4/[controller]")]
public class MedicosController : ControllerBase
{
    private readonly ClinicaDbContext _context;
    public MedicosController(ClinicaDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Medicos.Include(m => m.Especialidad).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Post(Medico m)
    {
        _context.Medicos.Add(m);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Médico creado correctamente", data = m });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Medico m)
    {
        if (id != m.IdMedico) return BadRequest();

        _context.Entry(m).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { message = "Médico actualizado" });
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Medicos.Any(e => e.IdMedico == id))
                return NotFound();

            throw;
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var m = await _context.Medicos.FindAsync(id);
        if (m == null) return NotFound();
        _context.Medicos.Remove(m);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Médico eliminado" });
    }
}
