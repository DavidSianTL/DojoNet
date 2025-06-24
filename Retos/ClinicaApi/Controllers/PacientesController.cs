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
public class PacientesController : ControllerBase
{

    private readonly PacienteDao _dao;
    private readonly ILogger<PacientesController> _logger;

    public PacientesController(PacienteDao dao, ILogger<PacientesController> logger)
    {
    _dao = dao;
    _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Obteniendo lista de todos los pacientes");
        var pacientes = await _dao.ObtenerTodosAsync();
        return Ok(new ApiResponse("200", "Lista de pacientes obtenida", pacientes));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Buscando paciente con ID {Id}", id);
        var paciente = await _dao.ObtenerPorIdAsync(id);
        if (paciente == null)
        {
            _logger.LogWarning("Paciente con ID {Id} no encontrado", id);
            return NotFound(new ApiResponse("404", "Paciente no encontrado"));
        }

        return Ok(new ApiResponse("200", "Paciente encontrado", paciente));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Paciente paciente)
    {
        if (!ModelState.IsValid)
        {
            var errores = ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            _logger.LogWarning("Datos inválidos al crear paciente: {@Errores}", errores);
            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

        _logger.LogInformation("Creando nuevo paciente: {Nombre}", paciente.Nombre);
        await _dao.CrearAsync(paciente);
        return Ok(new ApiResponse("201", "Paciente creado correctamente", paciente));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Paciente paciente)
    {
        if (!ModelState.IsValid)
        {
            var errores = ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            _logger.LogWarning("Datos inválidos al actualizar paciente ID {Id}: {@Errores}", id, errores);
            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

        _logger.LogInformation("Actualizando paciente ID {Id}", id);
        await _dao.ActualizarAsync(id, paciente);
        return Ok(new ApiResponse("200", "Paciente actualizado correctamente", paciente));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Eliminando paciente ID {Id}", id);
        await _dao.EliminarAsync(id);
        return Ok(new ApiResponse("200", "Paciente eliminado correctamente"));
    }
}
