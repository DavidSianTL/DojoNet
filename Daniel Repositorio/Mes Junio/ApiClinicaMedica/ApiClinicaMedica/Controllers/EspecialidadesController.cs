using ApiClinicaMedica.Data;
using ApiClinicaMedica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/v4/[controller]")]
public class EspecialidadesController : ControllerBase
{
    private readonly ClinicaDbContext _context;
    public EspecialidadesController(ClinicaDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Especialidades.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Post(Especialidad e)
    {
        _context.Especialidades.Add(e);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Especialidad creada", data = e });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _context.Especialidades.FindAsync(id);
        if (e == null) return NotFound();
        _context.Especialidades.Remove(e);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Especialidad eliminada" });
    }
}
