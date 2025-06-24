using ApiClinicaMedica.Dao;
using ApiClinicaMedica.Models;
using ApiClinicaMedica.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/v4/[controller]")]
public class EspecialidadesController : ControllerBase
{
    private readonly EspecialidadDAO _dao;

    public EspecialidadesController(EspecialidadDAO dao)
    {
        _dao = dao;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var lista = await _dao.ObtenerTodosAsync();
        return Ok(new ApiResponse<List<Especialidad>>(200, "Listado de especialidades", lista));
    }

    [HttpPost]
    public async Task<IActionResult> Post(Especialidad e)
    {
        await _dao.CrearAsync(e);
        return Ok(new ApiResponse<Especialidad>(200, "Especialidad creada correctamente", e));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _dao.EliminarAsync(id);
        if (!eliminado)
            return NotFound(new ApiResponse<string>(404, "Especialidad no encontrada"));
        return Ok(new ApiResponse<string>(200, "Especialidad eliminada correctamente"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Especialidad especialidad)
    {
        if (id != especialidad.IdEspecialidad) return BadRequest();

        var actualizado = await _dao.ActualizarAsync(id, especialidad);
        if (!actualizado) return NotFound();

        return Ok(new ApiResponse<Especialidad>(200, "Especialidad actualizada correctamente", especialidad));
    }


}
