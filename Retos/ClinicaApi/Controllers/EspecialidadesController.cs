using ClinicaApi.DAL;
using ClinicaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EspecialidadesController : ControllerBase
{
    private readonly EspecialidadDao _dao;

    public EspecialidadesController(IConfiguration config)
    {
        _dao = new EspecialidadDao(config);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var especialidades = await _dao.ObtenerTodasAsync();
        return Ok(new ApiResponse("200", "Lista de especialidades", especialidades));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var especialidad = await _dao.ObtenerPorIdAsync(id);
        if (especialidad == null)
        {
            return NotFound(new ApiResponse("404", "Especialidad no encontrada"));
        }

        return Ok(new ApiResponse("200", "Especialidad encontrada", especialidad));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Especialidad especialidad)
    {
        if (!ModelState.IsValid)
        {
            var errores = ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

        await _dao.CrearAsync(especialidad);
        return Ok(new ApiResponse("201", "Especialidad creada correctamente", especialidad));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Especialidad especialidad)
    {
        if (!ModelState.IsValid)
        {
            var errores = ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

        await _dao.ActualizarAsync(id, especialidad);
        return Ok(new ApiResponse("200", "Especialidad actualizada correctamente", especialidad));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _dao.EliminarAsync(id);
        return Ok(new ApiResponse("200", "Especialidad eliminada correctamente"));
    }
}
