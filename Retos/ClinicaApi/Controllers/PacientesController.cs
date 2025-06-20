using ClinicaApi.DAL;
using ClinicaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly PacienteDao _dao;

    public PacientesController(IConfiguration config)
    {
        _dao = new PacienteDao(config);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var pacientes = await _dao.ObtenerTodosAsync();
        return Ok(new ApiResponse("200", "Lista de pacientes obtenida", pacientes));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var paciente = await _dao.ObtenerPorIdAsync(id);
        if (paciente == null)
        {
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

            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

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

            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

        await _dao.ActualizarAsync(id, paciente);
        return Ok(new ApiResponse("200", "Paciente actualizado correctamente", paciente));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _dao.EliminarAsync(id);
        return Ok(new ApiResponse("200", "Paciente eliminado correctamente"));
    }
}
