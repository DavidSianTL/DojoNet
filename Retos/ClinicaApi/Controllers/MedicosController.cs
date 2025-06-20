using ClinicaApi.DAL;
using ClinicaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MedicosController : ControllerBase
{
    private readonly MedicoDao _dao;

    public MedicosController(IConfiguration config)
    {
        _dao = new MedicoDao(config);
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(new
    {
        ResponseCode = "200",
        ResponseMessage = "Lista de médicos obtenida",
        data = await _dao.ObtenerTodosAsync()
    });

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var medico = await _dao.ObtenerPorIdAsync(id);
        return medico == null
            ? NotFound(new { ResponseCode = "404", ResponseMessage = "Médico no encontrado" })
            : Ok(new { ResponseCode = "200", ResponseMessage = "Médico encontrado", data = medico });
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
    public async Task<IActionResult> Post(Medico medico)
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

        await _dao.CrearAsync(medico);
        return Ok(new
        {
            ResponseCode = "201",
            ResponseMessage = "Médico creado",
            data = medico
        });
    }

    [HttpPost]
    public async Task<IActionResult> Post(Medico medico)
    {
        await _dao.CrearAsync(medico);
        return Ok(new { ResponseCode = "201", ResponseMessage = "Médico creado correctamente", data = medico });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Medico medico)
    {
        await _dao.ActualizarAsync(id, medico);
        return Ok(new { ResponseCode = "200", ResponseMessage = "Médico actualizado", data = medico });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _dao.EliminarAsync(id);
        return Ok(new { ResponseCode = "200", ResponseMessage = "Médico eliminado" });
    }
}
