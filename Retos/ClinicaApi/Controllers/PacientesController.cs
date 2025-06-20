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
    public async Task<IActionResult> Get() => Ok(new
    {
        ResponseCode = "200",
        ResponseMessage = "Lista de pacientes",
        data = await _dao.ObtenerTodosAsync()
    });

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var paciente = await _dao.ObtenerPorIdAsync(id);
        return paciente == null
            ? NotFound(new { ResponseCode = "404", ResponseMessage = "Paciente no encontrado" })
            : Ok(new { ResponseCode = "200", ResponseMessage = "Paciente encontrado", data = paciente });
    }
    [HttpPost]
    public async Task<IActionResult> Post(Paciente paciente)
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


    [HttpPost]
    public async Task<IActionResult> Post(Paciente paciente)
    {
        await _dao.CrearAsync(paciente);
        return Ok(new { ResponseCode = "201", ResponseMessage = "Paciente creado", data = paciente });
    }
    [HttpPost]
    public async Task<IActionResult> Post(Paciente paciente)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                ResponseCode = "400",
                ResponseMessage = "Datos inválidos",
                Errors = ModelState.Values.SelectMany(e => e.Errors)
                                          .Select(e => e.ErrorMessage)
            });
        }

        await _dao.CrearAsync(paciente);
        return Ok(new
        {
            ResponseCode = "201",
            ResponseMessage = "Paciente creado",
            data = paciente
        });
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Paciente paciente)
    {
        await _dao.ActualizarAsync(id, paciente);
        return Ok(new { ResponseCode = "200", ResponseMessage = "Paciente actualizado", data = paciente });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _dao.EliminarAsync(id);
        return Ok(new { ResponseCode = "200", ResponseMessage = "Paciente eliminado" });
    }
}
