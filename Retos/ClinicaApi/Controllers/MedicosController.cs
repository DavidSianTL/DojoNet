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
public class MedicosController : ControllerBase
{
    private readonly MedicoDao _dao;
    private readonly ILogger<MedicosController> _logger;

    public MedicosController(MedicoDao dao, ILogger<MedicosController> logger)
    {
    _dao = dao;
    _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Obteniendo lista de todos los médicos");
        var medicos = await _dao.ObtenerTodosAsync();
        return Ok(new ApiResponse("200", "Lista de médicos obtenida", medicos));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Buscando médico con ID {Id}", id);
        var medico = await _dao.ObtenerPorIdAsync(id);
        if (medico == null)
        {
            _logger.LogWarning("Médico con ID {Id} no encontrado", id);
            return NotFound(new ApiResponse("404", "Médico no encontrado"));
        }

        return Ok(new ApiResponse("200", "Médico encontrado", medico));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Medico medico)
    {
        if (!ModelState.IsValid)
        {
            var errores = ModelState.Values
                                    .SelectMany(e => e.Errors)
                                    .Select(e => e.ErrorMessage)
                                    .ToList();

            _logger.LogWarning("Datos inválidos al crear médico: {@Errores}", errores);
            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

        _logger.LogInformation("Creando nuevo médico: {Nombre}", medico.Nombre);
        await _dao.CrearAsync(medico);
        return Ok(new ApiResponse("201", "Médico creado correctamente", medico));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Medico medico)
    {
        if (!ModelState.IsValid)
        {
            var errores = ModelState.Values
                                    .SelectMany(e => e.Errors)
                                    .Select(e => e.ErrorMessage)
                                    .ToList();

            _logger.LogWarning("Datos inválidos al actualizar médico ID {Id}: {@Errores}", id, errores);
            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

        _logger.LogInformation("Actualizando médico ID {Id}", id);
        await _dao.ActualizarAsync(id, medico);
        return Ok(new ApiResponse("200", "Médico actualizado", medico));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Eliminando médico ID {Id}", id);
        await _dao.EliminarAsync(id);
        return Ok(new ApiResponse("200", "Médico eliminado correctamente"));
    }
}
