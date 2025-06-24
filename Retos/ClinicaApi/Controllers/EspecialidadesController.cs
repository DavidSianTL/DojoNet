using ClinicaApi.DAL;
using ClinicaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClinicaApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EspecialidadesController : ControllerBase
{
    private readonly EspecialidadDao _dao;
    private readonly ILogger<EspecialidadesController> _logger;

    public EspecialidadesController(EspecialidadDao dao, ILogger<EspecialidadesController> logger)
    {
    _dao = dao;
    _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Solicitando todas las especialidades");
        var especialidades = await _dao.ObtenerTodasAsync();
        return Ok(new ApiResponse("200", "Lista de especialidades", especialidades));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Buscando especialidad con ID {Id}", id);
        var especialidad = await _dao.ObtenerPorIdAsync(id);
        if (especialidad == null)
        {
            _logger.LogWarning("Especialidad con ID {Id} no encontrada", id);
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

            _logger.LogWarning("Error al crear especialidad. Datos inválidos: {@Errores}", errores);
            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

        _logger.LogInformation("Creando nueva especialidad: {Nombre}", especialidad.Nombre);
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

            _logger.LogWarning("Error al actualizar especialidad ID {Id}. Datos inválidos: {@Errores}", id, errores);
            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

        _logger.LogInformation("Actualizando especialidad ID {Id}", id);
        await _dao.ActualizarAsync(id, especialidad);
        return Ok(new ApiResponse("200", "Especialidad actualizada correctamente", especialidad));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Eliminando especialidad ID {Id}", id);
        await _dao.EliminarAsync(id);
        return Ok(new ApiResponse("200", "Especialidad eliminada correctamente"));
    }
}
