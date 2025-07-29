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
        private readonly daoFeriados _daoFeriados;

        public FeriadosController(daoFeriados daoFeriados)
        {
            _daoFeriados = daoFeriados;
        }

        public async Task<IActionResult> Index()
        {
            var modelo = new GestionFeriadosViewModel
            {
                FeriadosFijos = await _daoFeriados.ListarFeriadosFijos(),
                FeriadosVariables = await _daoFeriados.ListarFeriadosVariables(),
                TiposFeriado = await _daoFeriados.ListarTiposFeriado()
            };
            return View(modelo);
        }


        [AuthorizeRole("Empleado", "SuperAdministrador")]
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

        [AuthorizeRole("Empleado", "SuperAdministrador")]
        public async Task<IActionResult> _FeriadoVariableForm(int? id)
        {
            FeriadoVariableViewModel modelo;
            if (id.HasValue)
            {
                modelo = await _daoFeriados.ObtenerFeriadoVariable(id.Value);
                if (modelo == null) return NotFound();
            }
            else
            {
                modelo = new FeriadoVariableViewModel { Fecha = DateTime.Today };
            }

            var tiposFeriadoList = await _daoFeriados.ListarTiposFeriado() ?? new List<TipoFeriadoViewModel>();
            var tiposFeriadoValidos = tiposFeriadoList
                .Where(t => t != null && t.TipoFeriadoId > 0 && !string.IsNullOrEmpty(t.Nombre))
                .ToList();
            ViewBag.TiposFeriado = new SelectList(tiposFeriadoValidos, "TipoFeriadoId", "Nombre", modelo.TipoFeriadoId);
            return PartialView("_FeriadoVariableForm", modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("Empleado", "SuperAdministrador")]
        public async Task<IActionResult> GuardarFeriadoFijo(FeriadoFijoViewModel model)
        {
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

            model.Usr_creacion = User.Identity.Name ?? "AdminDev";
            model.Usr_modifica = User.Identity.Name ?? "AdminDev";

            var mensaje = await _daoFeriados.MantFeriadoFijo(model);
            bool exito = !mensaje.ToLower().Contains("error");

            return Json(new { success = exito, message = mensaje });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("Empleado", "SuperAdministrador")]
        public async Task<IActionResult> GuardarFeriadoVariable(FeriadoVariableViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var tiposFeriadoList = await _daoFeriados.ListarTiposFeriado() ?? new List<TipoFeriadoViewModel>();
                var tiposFeriadoValidos = tiposFeriadoList
                    .Where(t => t != null && t.TipoFeriadoId > 0 && !string.IsNullOrEmpty(t.Nombre))
                    .ToList();
                ViewBag.TiposFeriado = new SelectList(tiposFeriadoValidos, "TipoFeriadoId", "Nombre", model.TipoFeriadoId);
                return PartialView("_FeriadoVariableForm", model);
            }

            model.Usr_creacion = User.Identity.Name ?? "AdminDev";
            model.Usr_modifica = User.Identity.Name ?? "AdminDev";

            var mensaje = await _daoFeriados.MantFeriadoVariable(model);
            bool exito = !mensaje.ToLower().Contains("error");

            return Json(new { success = exito, message = mensaje });
        }
    }
}
