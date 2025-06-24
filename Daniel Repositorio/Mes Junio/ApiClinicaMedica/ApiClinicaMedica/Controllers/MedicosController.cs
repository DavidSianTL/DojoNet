using ApiClinicaMedica.Dao;
using ApiClinicaMedica.Models;
using ApiClinicaMedica.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/v4/[controller]")]
public class MedicosController : ControllerBase
{
    private readonly MedicoDAO _dao;

    public MedicosController(MedicoDAO dao)
    {
        _dao = dao;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var medicos = await _dao.ObtenerMedicosAsync();
        return Ok(new ApiResponse<List<Medico>>(200, "Listado de médicos", medicos));
    }

    [HttpPost]
    public async Task<IActionResult> Post(Medico m)
    {
        await _dao.CrearAsync(m);
        return Ok(new ApiResponse<Medico>(200, "Médico creado correctamente", m));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Medico m)
    {
        var actualizado = await _dao.ActualizarAsync(id, m);
        if (!actualizado)
            return NotFound(new ApiResponse<string>(404, "Médico no encontrado"));
        return Ok(new ApiResponse<string>(200, "Médico actualizado correctamente"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _dao.EliminarAsync(id);
        if (!eliminado)
            return NotFound(new ApiResponse<string>(404, "Médico no encontrado"));
        return Ok(new ApiResponse<string>(200, "Médico eliminado correctamente"));
    }
}
