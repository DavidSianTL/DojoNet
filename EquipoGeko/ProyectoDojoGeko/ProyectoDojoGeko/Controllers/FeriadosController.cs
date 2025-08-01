using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class FeriadosController : Controller
    {
        //  Controlador para gestionar feriados fijos y variables
        private readonly daoFeriados _daoFeriados;

        public FeriadosController(daoFeriados daoFeriados)
        {
            _daoFeriados = daoFeriados;
        }

        public async Task<IActionResult> Index()
        {
            // Vista de menú principal - no necesita cargar datos
            return View();
        }

        // Vista dedicada para gestión de Feriados Fijos
        public async Task<IActionResult> FeriadosFijos()
        {
            try
            {
                var model = new GestionFeriadosViewModel();
                
                // Cargar feriados fijos
                model.FeriadosFijos = await _daoFeriados.ListarFeriadosFijos();
                
                // Cargar tipos de feriado para el formulario
                var tiposFeriado = await _daoFeriados.ListarTiposFeriado();
                model.TiposFeriado = tiposFeriado?.Where(t => t != null && 
                                                              !string.IsNullOrEmpty(t.Nombre) && 
                                                              t.TipoFeriadoId > 0).ToList() ?? new List<TipoFeriadoViewModel>();

                // Configurar ViewBag para el formulario
                ViewBag.TiposFeriado = new SelectList(model.TiposFeriado, "TipoFeriadoId", "Nombre");

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los feriados fijos: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Vista dedicada para gestión de Feriados Variables
        public async Task<IActionResult> FeriadosVariables()
        {
            try
            {
                var model = new GestionFeriadosViewModel();
                
                // Cargar feriados variables
                model.FeriadosVariables = await _daoFeriados.ListarFeriadosVariables();
                
                // Cargar tipos de feriado para el formulario
                var tiposFeriado = await _daoFeriados.ListarTiposFeriado();
                model.TiposFeriado = tiposFeriado?.Where(t => t != null && 
                                                              !string.IsNullOrEmpty(t.Nombre) && 
                                                              t.TipoFeriadoId > 0).ToList() ?? new List<TipoFeriadoViewModel>();

                // Configurar ViewBag para el formulario
                ViewBag.TiposFeriado = new SelectList(model.TiposFeriado, "TipoFeriadoId", "Nombre");

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los feriados variables: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [AuthorizeRole("Empleado", "SuperAdministrador")]
        // Método para mostrar el formulario de feriado variable
        public async Task<IActionResult> _FeriadoFijoForm(int? dia, int? mes, int? tipoFeriadoId)
        {
            FeriadoFijoViewModel modelo;
            if (dia.HasValue && mes.HasValue && tipoFeriadoId.HasValue)
            {
                modelo = await _daoFeriados.ObtenerFeriadoFijo(dia.Value, mes.Value, tipoFeriadoId.Value);
                if (modelo == null) return NotFound();
                modelo.Original_Dia = modelo.Dia;
                modelo.Original_Mes = modelo.Mes;
                modelo.Original_TipoFeriadoId = modelo.TipoFeriadoId;
            }
            else
            {
                modelo = new FeriadoFijoViewModel();
            }

            var tiposFeriadoList = await _daoFeriados.ListarTiposFeriado() ?? new List<TipoFeriadoViewModel>();
            var tiposFeriadoValidos = tiposFeriadoList
                .Where(t => t != null && t.TipoFeriadoId > 0 && !string.IsNullOrEmpty(t.Nombre))
                .ToList();
            ViewBag.TiposFeriado = new SelectList(tiposFeriadoValidos, "TipoFeriadoId", "Nombre", modelo.TipoFeriadoId);
            return PartialView("_FeriadoFijoForm", modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("Empleado", "SuperAdministrador")]
        // Método para guardar feriado fijo
        public async Task<IActionResult> GuardarFeriadoFijo(FeriadoFijoViewModel model)
        {
            ModelState.Remove("TipoFeriadoNombre");
            ModelState.Remove("Usr_creacion");
            ModelState.Remove("Usr_modifica");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                return Json(new { success = false, errors = errors });
            }

            string mensaje;
            bool isUpdate = model.Original_Dia.HasValue && model.Original_Mes.HasValue && model.Original_TipoFeriadoId.HasValue;

            try
            {
                if (isUpdate)
                {
                    model.Usr_modifica = User.Identity?.Name ?? "AdminDev";
                    mensaje = await _daoFeriados.ActualizarFeriadoFijo(model);
                }
                else
                {
                    model.Usr_creacion = User.Identity?.Name ?? "AdminDev";
                    mensaje = await _daoFeriados.InsertarFeriadoFijo(model);
                }

                bool exito = !mensaje.ToLower().Contains("error");

                if (exito)
                {
                    TempData["Success"] = mensaje;
                    return RedirectToAction(nameof(FeriadosFijos));
                }
                else
                {
                    TempData["Error"] = mensaje;
                    // Considera redirigir a una vista con el formulario y los errores
                    return RedirectToAction(nameof(FeriadosFijos));
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error interno: " + ex.Message;
                return RedirectToAction(nameof(FeriadosFijos));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("Empleado", "SuperAdministrador")]
        // Método para mostrar el formulario de feriado variable
        public async Task<IActionResult> GuardarFeriadoVariable(FeriadoVariableViewModel model)
        {
            // Remover validaciones de campos que no vienen del formulario
            ModelState.Remove("Usr_creacion");
            ModelState.Remove("Usr_modifica");
            ModelState.Remove("TipoFeriadoNombre");

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return Json(new { success = false, errors = errors });
            }

            // Configurar campos de auditoría
            model.Usr_creacion = User.Identity.Name ?? "AdminDev";
            model.Usr_modifica = User.Identity.Name ?? "AdminDev";

            try
            {
                var mensaje = await _daoFeriados.MantFeriadoVariable(model);
                bool exito = !mensaje.ToLower().Contains("error");

                if (exito)
                {
                    TempData["Success"] = mensaje;
                    return RedirectToAction(nameof(FeriadosVariables));
                }
                else
                {
                    TempData["Error"] = mensaje;
                    return RedirectToAction(nameof(FeriadosVariables));
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error interno: " + ex.Message;
                return RedirectToAction(nameof(FeriadosVariables));
            }
        }

        // Método para eliminar feriado fijo
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("Empleado", "SuperAdministrador")]
        // Eliminar un feriado fijo
        public async Task<IActionResult> EliminarFeriadoFijo(int dia, int mes, int tipoFeriadoId)
        {
            try
            {
                var model = new FeriadoFijoViewModel
                {
                    Dia = dia,
                    Mes = mes,
                    TipoFeriadoId = tipoFeriadoId
                };

                var mensaje = await _daoFeriados.EliminarFeriadoFijo(model);
                bool exito = !mensaje.ToLower().Contains("error");

                return Json(new { success = exito, message = mensaje });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar: " + ex.Message });
            }
        }
    }
}
