using ClinicaApi.DAL;
using ClinicaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private readonly CitaDao _dao;

    public CitasController(IConfiguration config)
    {
        _dao = new CitaDao(config);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var citas = await _dao.ObtenerTodasAsync();
        return Ok(new ApiResponse("200", "Lista de citas", citas));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cita = await _dao.ObtenerPorIdAsync(id);
        if (cita == null)
        {
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

            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

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

            return BadRequest(new ApiResponse("400", "Datos inválidos", errores));
        }

        await _dao.ActualizarAsync(id, cita);
        return Ok(new ApiResponse("200", "Cita actualizada correctamente", cita));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _dao.EliminarAsync(id);
        return Ok(new ApiResponse("200", "Cita eliminada correctamente"));
    }
}
