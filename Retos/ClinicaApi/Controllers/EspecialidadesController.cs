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
    public async Task<IActionResult> Get() => Ok(new
    {
        ResponseCode = "200",
        ResponseMessage = "Lista de especialidades",
        data = await _dao.ObtenerTodasAsync()
    });

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var especialidad = await _dao.ObtenerPorIdAsync(id);
        return especialidad == null
            ? NotFound(new { ResponseCode = "404", ResponseMessage = "Especialidad no encontrada" })
            : Ok(new { ResponseCode = "200", ResponseMessage = "Especialidad encontrada", data = especialidad });
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
    public async Task<IActionResult> Post(Especialidad especialidad)
    {
        await _dao.CrearAsync(especialidad);
        return Ok(new { ResponseCode = "201", ResponseMessage = "Especialidad creada", data = especialidad });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Especialidad especialidad)
    {
        await _dao.ActualizarAsync(id, especialidad);
        return Ok(new { ResponseCode = "200", ResponseMessage = "Especialidad actualizada", data = especialidad });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _dao.EliminarAsync(id);
        return Ok(new { ResponseCode = "200", ResponseMessage = "Especialidad eliminada" });
    }
}
