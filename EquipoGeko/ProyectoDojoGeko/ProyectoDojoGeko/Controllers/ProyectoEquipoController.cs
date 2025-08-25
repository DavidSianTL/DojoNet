using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Services;

namespace ProyectoDojoGeko.Controllers
{
    public class ProyectoEquipoController : Controller
    {
        private readonly daoProyectoEquipoWSAsync _daoProyectoEquipo;
        private readonly IBitacoraService _bitacoraService;

        public ProyectoEquipoController(daoProyectoEquipoWSAsync daoProyectoEquipo, IBitacoraService bitacoraService)
        {
            _daoProyectoEquipo = daoProyectoEquipo;
            _bitacoraService = bitacoraService;
        }


        // ========================== PROYECTOS ==========================

    #region PROYECTOS
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var proyectos = await _daoProyectoEquipo.ObtenerProyectosAsync();
                if (proyectos == null) return NotFound();

                await _bitacoraService.RegistrarBitacoraAsync("Vista ProyectoEquipo", "Se accedió a la vista proyectoEquipo");
                return View(proyectos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        private async Task CargarEstadosEnViewBagAsync()
        {
            var estados = await _daoProyectoEquipo.ListarEstadosComboAsync();
            ViewBag.Estados = estados;
        }


        [HttpGet]
        public async Task<IActionResult> CrearProyecto()
        {
            var nuevoProyecto = new ProyectoViewModel();
            await CargarEstadosEnViewBagAsync();
            await _bitacoraService.RegistrarBitacoraAsync("Vista CrearProyecto", "Se accedió a la vista CrearProyecto");
            return View(nuevoProyecto);
        }

        [HttpPost]
        public async Task<IActionResult> CrearProyecto(ProyectoViewModel proyecto)
        {
            if (!ModelState.IsValid)
            {
                await CargarEstadosEnViewBagAsync();
                return View(proyecto);
            }

            try
            {
                await _daoProyectoEquipo.InsertarProyectoAsync(proyecto);
                
                // Registrar cambio
                await _bitacoraService.RegistrarBitacoraAsync("Vista CrearProyecto", $"Se creó el proyecto: {proyecto.Nombre} exitosamente");
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear el proyecto: {ex.Message}");
                await CargarEstadosEnViewBagAsync();
                return View(proyecto);
            }
        }



        [HttpGet]
        public async Task<IActionResult> EditarProyecto(int id)
        {
            var proyecto = await _daoProyectoEquipo.ObtenerProyectoPorIdAsync(id);
            if (proyecto == null) return NotFound();
            await CargarEstadosEnViewBagAsync();

            await _bitacoraService.RegistrarBitacoraAsync("Vista EditarProyecto", "Se accedió a la vista EditarProyecto");
            return View(proyecto);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProyecto(ProyectoViewModel proyecto)
        {
            if (!ModelState.IsValid) return View(proyecto);

            try
            {
                await _daoProyectoEquipo.ActualizarProyectoAsync(proyecto);

                // Registrar cambio
                await _bitacoraService.RegistrarBitacoraAsync("Vista EditarProyecto", $"Se editó el proyecto con el Id: {proyecto.IdProyecto} exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al editar el proyecto: {ex.Message}");
                return View(proyecto);
            }
        }


        [HttpPost]
        public async Task<IActionResult> EliminarProyecto(int id)
        {
            try
            {
                await _daoProyectoEquipo.EliminarProyectoAsync(id);
                // Registrar cambios
                await _bitacoraService.RegistrarBitacoraAsync("EliminarProyecto", $"Se eliminó el proyecto con el Id: {id} exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el proyecto: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<ActionResult> Detalle(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "ID de proyecto inválido.";
                    return RedirectToAction(nameof(Index));
                }

                var proyecto = await _daoProyectoEquipo.ObtenerProyectoPorIdAsync(id);

                if (proyecto == null)
                {
                    TempData["ErrorMessage"] = "Proyecto no encontrado.";
                    return RedirectToAction(nameof(Index));
                }


                await _bitacoraService.RegistrarBitacoraAsync("Vista DetalleProyecto", "Se accedió a la vista DetalleProyecto");
                return View(nameof(Detalle), proyecto); 
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los datos: {ex.ToString()}";
                return RedirectToAction(nameof(Index));
            }
        }


#endregion


        // ========================== EQUIPOS ==========================

    #region EQUIPOS

        [HttpGet]
        public async Task<IActionResult> Equipos()
        {
            try
            {
                var equipos = await _daoProyectoEquipo.ObtenerEquiposAsync();
                return View(equipos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult CrearEquipo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearEquipo(EquipoViewModel equipo)
        {
            if (!ModelState.IsValid) return View(equipo);

            try
            {
                await _daoProyectoEquipo.InsertarEquipoAsync(equipo);
                return RedirectToAction("Equipos");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear el equipo: {ex.Message}");
                return View(equipo);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditarEquipo(int id)
        {
            var equipo = await _daoProyectoEquipo.ObtenerEquipoPorIdAsync(id);
            if (equipo == null) return NotFound();
            return View(equipo);
        }

        [HttpPost]
        public async Task<IActionResult> EditarEquipo(EquipoViewModel equipo)
        {
            if (!ModelState.IsValid) return View(equipo);

            try
            {
                await _daoProyectoEquipo.ActualizarEquipoAsync(equipo);
                return RedirectToAction("Equipos");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al editar el equipo: {ex.Message}");
                return View(equipo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarEquipo(int id)
        {
            try
            {
                await _daoProyectoEquipo.EliminarEquipoAsync(id);
                return RedirectToAction("Equipos");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el equipo: {ex.Message}");
            }
        }

        #endregion


        // ========================== ASIGNACIONES ==========================

    #region ASIGNACIONES

        [HttpPost]
        public async Task<ActionResult<int>> AsignarEquipoAProyecto([FromBody] AsignacionRequest request)
        {
            try
            {
                if (request.IdProyecto <= 0 || request.IdEquipo <= 0)
                    return BadRequest("Los IDs deben ser válidos.");

                var result = await _daoProyectoEquipo.AsignarEquipoAProyectoAsync(request.IdProyecto, request.IdEquipo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en la asignación: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipoViewModel>>> GetEquiposPorProyecto(int idProyecto)
        {
            try
            {
                if (idProyecto <= 0)
                    return BadRequest("El ID del proyecto debe ser válido");

                var equipos = await _daoProyectoEquipo.ObtenerEquiposPorProyectoAsync(idProyecto);
                return Ok(equipos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }

    public class AsignacionRequest
    {
        public int IdProyecto { get; set; }
        public int IdEquipo { get; set; }
    }

    #endregion

}
