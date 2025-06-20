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
    public async Task<IActionResult> Get()
    {
        var medicos = await _dao.ObtenerTodosAsync();
        return Ok(new ApiResponse("200", "Lista de m�dicos obtenida", medicos));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var medico = await _dao.ObtenerPorIdAsync(id);
        if (medico == null)
        {
            return NotFound(new ApiResponse("404", "M�dico no encontrado"));
        }

        return Ok(new ApiResponse("200", "M�dico encontrado", medico));
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

            return BadRequest(new ApiResponse("400", "Datos inv�lidos", errores));
        }

        await _dao.CrearAsync(medico);
        return Ok(new ApiResponse("201", "M�dico creado correctamente", medico));
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

            return BadRequest(new ApiResponse("400", "Datos inv�lidos", errores));
        }

        await _dao.ActualizarAsync(id, medico);
        return Ok(new ApiResponse("200", "M�dico actualizado", medico));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _dao.EliminarAsync(id);
        return Ok(new ApiResponse("200", "M�dico eliminado correctamente"));
    }
}
