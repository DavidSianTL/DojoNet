using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class EmpleadosController : Controller
    {
        private readonly daoEmpleadoWSAsync _daoEmpleado;
        private readonly daoUsuarioWSAsync _daoUsuario;
        private readonly daoRolesWSAsync _daoRoles;
        private readonly daoSistemaWSAsync _daoSistema;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly ILoggingService _loggingService;

        public EmpleadosController(
            daoEmpleadoWSAsync daoEmpleado,
            daoUsuarioWSAsync daoUsuario,
            daoRolesWSAsync daoRoles,
            daoSistemaWSAsync daoSistema,
            daoBitacoraWSAsync daoBitacora,
            ILoggingService loggingService)
        {
            _daoEmpleado = daoEmpleado;
            _daoUsuario = daoUsuario;
            _daoRoles = daoRoles;
            _daoSistema = daoSistema;
            _daoBitacora = daoBitacora;
            _loggingService = loggingService;
        }

        private async Task RegistrarBitacora(string accion, string descripcion)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = $"{descripcion} | Usuario: {usuario}",
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }

        private async Task RegistrarError(string accion, Exception ex)
        {
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            await _loggingService.RegistrarLogAsync(new LogViewModel
            {
                Accion = $"Error {accion}",
                Descripcion = $"Error al {accion} por {usuario}: {ex.Message}",
                Estado = false
            });
        }
        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> CreateEdit(EmpleadoViewModel empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (empleado.IdEmpleado == 0)
                    {
                        await _daoEmpleado.InsertarEmpleadoAsync(empleado);
                        await RegistrarBitacora("Crear Empleado", $"Nuevo empleado creado: {empleado.NombreEmpleado} {empleado.ApellidoEmpleado}");
                    }
                    else
                    {
                        await _daoEmpleado.ActualizarEmpleadoAsync(empleado);
                        await RegistrarBitacora("Actualizar Empleado", $"ID: {empleado.IdEmpleado} - {empleado.NombreEmpleado} {empleado.ApellidoEmpleado}");
                    }
                    return RedirectToAction("Index");
                }
                return View(empleado);
            }
            catch (Exception ex)
            {
                string accion = empleado.IdEmpleado == 0 ? "crear empleado" : "actualizar empleado";
                await RegistrarError(accion, ex);
                return View(empleado);
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> ELIMINAR(int id)
        {
            try
            {
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);
                if (empleado == null)
                {
                    await RegistrarError("eliminar empleado", new Exception($"Empleado {id} no encontrado"));
                    return NotFound();
                }

                await RegistrarBitacora("Vista Eliminar Empleado", $"ID: {id}");
                return View(empleado);
            }
            catch (Exception ex)
            {
                await RegistrarError("eliminar empleado", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> ELIMINAR(int id, IFormCollection collection)
        {
            try
            {
                await _daoEmpleado.EliminarEmpleadoAsync(id);
                await RegistrarBitacora("Empleado Eliminado", $"ID: {id}");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("eliminar empleado", ex);
                return RedirectToAction("ELIMINAR", new { id });
            }
        }
    }
}

