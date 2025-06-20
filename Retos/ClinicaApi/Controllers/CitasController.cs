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
    public async Task<IActionResult> Get() => Ok(new
    {
        ResponseCode = "200",
        ResponseMessage = "Lista de citas",
        data = await _dao.ObtenerTodasAsync()
    });
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


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cita = await _dao.ObtenerPorIdAsync(id);
        return cita == null
            ? NotFound(new { ResponseCode = "404", ResponseMessage = "Cita no encontrada" })
            : Ok(new { ResponseCode = "200", ResponseMessage = "Cita encontrada", data = cita });
    }

    [HttpPost]
    public async Task<IActionResult> Post(Cita cita)
    {
        await _dao.CrearAsync(cita);
        return Ok(new { ResponseCode = "201", ResponseMessage = "Cita creada", data = cita });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Cita cita)
    {
        await _dao.ActualizarAsync(id, cita);
        return Ok(new { ResponseCode = "200", ResponseMessage = "Cita actualizada", data = cita });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _dao.EliminarAsync(id);
        return Ok(new { ResponseCode = "200", ResponseMessage = "Cita eliminada" });
    }
}
