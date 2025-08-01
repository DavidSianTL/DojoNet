using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Controllers
{
  
    public class ProyectoEquipoController : ControllerBase
    {
        private readonly daoProyectoEquipoWSAsync _dao;

        public ProyectoEquipoController(daoProyectoEquipoWSAsync dao)
        {
            _dao = dao;
        }

        // PROYECTOS

        
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var proyectos = await _dao.ObtenerProyectosAsync();

                if (proyectos == null)
                {
                    return NotFound();
                }

                return Ok(proyectos);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

      
        [HttpPost("proyectos")]
        public async Task<ActionResult<int>> PostProyecto([FromBody] ProyectoViewModel proyecto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _dao.InsertarProyectoAsync(proyecto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // EQUIPOS

       
        [HttpGet("equipos")]
        public async Task<ActionResult<IEnumerable<EquipoViewModel>>> GetEquipos()
        {
            try
            {
                var equipos = await _dao.ObtenerEquiposAsync();
                return Ok(equipos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

       
        [HttpPost("equipos")]
        public async Task<ActionResult<int>> PostEquipo([FromBody] EquipoViewModel equipo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _dao.InsertarEquipoAsync(equipo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // ASIGNACIONES PROYECTO-EQUIPO

     
        [HttpPost("asignaciones")]
        public async Task<ActionResult<int>> AsignarEquipoAProyecto([FromBody] AsignacionRequest request)
        {
            try
            {
                if (request.IdProyecto <= 0 || request.IdEquipo <= 0)
                {
                    return BadRequest("Los IDs de proyecto y equipo deben ser válidos");
                }

                var result = await _dao.AsignarEquipoAProyectoAsync(request.IdProyecto, request.IdEquipo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        
        [HttpGet("proyectos/{idProyecto}/equipos")]
        public async Task<ActionResult<IEnumerable<EquipoViewModel>>> GetEquiposPorProyecto(int idProyecto)
        {
            try
            {
                if (idProyecto <= 0)
                {
                    return BadRequest("El ID del proyecto debe ser válido");
                }

                var equipos = await _dao.ObtenerEquiposPorProyectoAsync(idProyecto);
                return Ok(equipos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }

    // Clase auxiliar para el request de asignación
    public class AsignacionRequest
    {
        public int IdProyecto { get; set; }
        public int IdEquipo { get; set; }
    }
}