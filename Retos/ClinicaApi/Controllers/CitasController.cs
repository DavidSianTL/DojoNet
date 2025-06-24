using ClinicaApi.DAL;
using ClinicaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ClinicaApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private readonly CitaDao _dao;
    private readonly ILogger<CitasController> _logger;

    public CitasController(CitaDao dao, ILogger<CitasController> logger)
    {
        _dao = dao;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Obteniendo todas las citas");
        var citas = await _dao.ObtenerTodasAsync();
        return Ok(new ApiResponse("200", "Lista de citas", citas));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Buscando cita con ID {Id}", id);
        var cita = await _dao.ObtenerPorIdAsync(id);
        if (cita == null)
        {
            _logger.LogWarning("Cita con ID {Id} no encontrada", id);
            return NotFound(new ApiResponse("404", "Cita no encontrada"));
        }

        return Ok(new ApiResponse("200", "Cita encontrada", cita));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Cita cita)
    {
        if (!ModelState.IsValid)
        {
            var errores = ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            _logger.LogWarning("Datos inválidos al crear cita: {@Errores}", errores);
            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

        _logger.LogInformation("Creando nueva cita para PacienteId {PacienteId}", cita.PacienteId);
        await _dao.CrearAsync(cita);
        return Ok(new ApiResponse("201", "Cita creada correctamente", cita));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Cita cita)
    {
        if (!ModelState.IsValid)
        {
            var errores = ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            _logger.LogWarning("Datos inválidos al actualizar cita ID {Id}: {@Errores}", id, errores);
            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

        _logger.LogInformation("Actualizando cita ID {Id}", id);
        await _dao.ActualizarAsync(id, cita);
        return Ok(new ApiResponse("200", "Cita actualizada correctamente", cita));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Eliminando cita ID {Id}", id);
        await _dao.EliminarAsync(id);
        return Ok(new ApiResponse("200", "Cita eliminada correctamente"));
    }
}
