using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class EmpleadosController : Controller
    {
        private readonly daoEmpleadoWSAsync _daoEmpleado;
        private readonly daoUsuarioWSAsync _daoUsuario;
        private readonly daoRolesWSAsync _daoRoles;
        private readonly daoSistemaWSAsync _daoSistema;
        private readonly daoLogWSAsync _daoLog;
        private readonly daoBitacoraWSAsync _daoBitacora;

        public EmpleadosController()
        {
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            _daoEmpleado = new daoEmpleadoWSAsync(connectionString);
            _daoUsuario = new daoUsuarioWSAsync(connectionString);
            _daoRoles = new daoRolesWSAsync(connectionString);
            _daoSistema = new daoSistemaWSAsync(connectionString);
            _daoLog = new daoLogWSAsync(connectionString);
            _daoBitacora = new daoBitacoraWSAsync(connectionString);
        }

        private async Task RegistrarError(string accion, Exception ex)
        {
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            await _daoLog.InsertarLogAsync(new LogViewModel
            {
                Accion = $"Error {accion}",
                Descripcion = $"Error al {accion} por {usuario}: {ex.Message}",
                Estado = false
            });
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

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor","Visualizador")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();
                await RegistrarBitacora("Vista Empleados", "Acceso a lista de empleados");
                return View(empleados ?? new List<EmpleadoViewModel>());
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a vista de empleados", ex);
                return View(new List<EmpleadoViewModel>());
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> LISTAR(int id)
        {
            try
            {
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);
                if (empleado == null)
                {
                    await RegistrarError("listar empleado", new Exception($"Empleado {id} no encontrado"));
                    return NotFound();
                }
                await RegistrarBitacora("Listar Empleado", $"ID: {id}");
                return View(empleado);
            }
            catch (Exception ex)
            {
                await RegistrarError("listar empleado", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);
                if (empleado == null)
                {
                    await RegistrarError("ver detalle empleado", new Exception($"Empleado {id} no encontrado"));
                    return NotFound();
                }
                await RegistrarBitacora("Detalle Empleado", $"ID: {id}");
                return View(empleado);
            }
            catch (Exception ex)
            {
                await RegistrarError("ver detalle empleado", ex);
                return RedirectToAction("Index");
            }
        }

        // NUEVA ACCIÓN GET para CreateEdit - Maneja tanto crear como editar
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> CreateEdit(int? id)
        {
            try
            {
                if (id.HasValue && id.Value > 0)
                {
                    // Es edición - obtener el empleado existente
                    var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id.Value);
                    if (empleado == null)
                    {
                        await RegistrarError("editar empleado", new Exception($"Empleado {id} no encontrado"));
                        return NotFound();
                    }
                    await RegistrarBitacora("Editar Empleado", $"ID: {id}");
                    return View(empleado);
                }
                else
                {
                    // Es creación - devolver modelo vacío
                    await RegistrarBitacora("Crear Empleado", "Acceso a formulario de creación");
                    return View(new EmpleadoViewModel());
                }
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a formulario empleado", ex);
                return RedirectToAction("Index");
            }
        }

        // NUEVA ACCIÓN POST para CreateEdit - Maneja tanto crear como editar
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
                        // Es creación
                        await _daoEmpleado.InsertarEmpleadoAsync(empleado);
                        await RegistrarBitacora("Crear Empleado", $"Nuevo empleado creado: {empleado.NombreEmpleado} {empleado.ApellidoEmpleado}");
                    }
                    else
                    {
                        // Es edición
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
        public IActionResult CREAR()
        {
            return View(new EmpleadoViewModel());
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> CREAR(EmpleadoViewModel empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoEmpleado.InsertarEmpleadoAsync(empleado);
                    await RegistrarBitacora("Crear Empleado", $"Nuevo empleado creado");
                    return RedirectToAction("Index");
                }
                return View(empleado);
            }
            catch (Exception ex)
            {
                await RegistrarError("crear empleado", ex);
                return View(empleado);
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> EDITAR(int id)
        {
            try
            {
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);
                if (empleado == null)
                {
                    await RegistrarError("editar empleado", new Exception($"Empleado {id} no encontrado"));
                    return NotFound();
                }
                await RegistrarBitacora("Editar Empleado", $"ID: {id}");
                return View(empleado);
            }
            catch (Exception ex)
            {
                await RegistrarError("editar empleado", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> EDITAR(EmpleadoViewModel empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoEmpleado.ActualizarEmpleadoAsync(empleado);
                    await RegistrarBitacora("Actualizar Empleado", $"ID: {empleado.IdEmpleado}");
                    return RedirectToAction("Index");
                }
                return View(empleado);
            }
            catch (Exception ex)
            {
                await RegistrarError("actualizar empleado", ex);
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
                await RegistrarBitacora("Eliminar Empleado", $"ID: {id}");
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