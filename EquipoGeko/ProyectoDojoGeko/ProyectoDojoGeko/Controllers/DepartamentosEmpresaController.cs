using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.DepartamentosEmpresa;
using System.Collections.Generic;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class DepartamentosEmpresaController : Controller
    {

        // Instanciamos el DAO de Departamentos y Empresa
        private readonly daoDepartamentosEmpresaWSAsync _daoDepartamentosEmpresa;

        // Instanciamos el DAO de Departamento
        private readonly daoDepartamentoWSAsync _daoDepartamento;

        // Instanciamos el DAO de Empresa
        private readonly daoEmpresaWSAsync _daoEmpresa;

        // Instanciamos el DAO de Bitacora
        private readonly daoBitacoraWSAsync _daoBitacora;

        // Instanciamos el DAO de Log
        private readonly daoLogWSAsync _daoLog;

        public DepartamentosEmpresaController()
        {
            // Cadena de conexión a la base de datos - ACTUALIZADA
            string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            //string _connectionString = "Server=NEWPEGHOSTE\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";

            // Inicializar el DAO con la cadena de conexión
            _daoDepartamentosEmpresa = new daoDepartamentosEmpresaWSAsync(_connectionString);
            _daoDepartamento = new daoDepartamentoWSAsync(_connectionString);
            _daoEmpresa = new daoEmpresaWSAsync(_connectionString);
            _daoBitacora = new daoBitacoraWSAsync(_connectionString);
            _daoLog = new daoLogWSAsync(_connectionString);
        }

        // Método privado para registrar errores en Log
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

        // Método privado para registrar acciones exitosas en Bitácora
        private async Task RegistrarBitacora(string accion, string descripcion)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = descripcion,
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }

        // Acción que muestra la vista de departamentos empresa
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {

                // Obtenemos los datos necesarios
                var departamentosEmpresa = await _daoDepartamentosEmpresa.ObtenerDepartamentosEmpresa();

                // Validamos que existan departamentos empresa
                if (departamentosEmpresa.Count == 0)
                {
                    ViewBag.Mensaje = "No se encontraron departamentos empresa.";

                    // Si no hay registros, devolvemos una lista vacía
                    return View(new List<DepartamentoEmpresaViewModel>());
                }

                // Registro en bitácora
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuario = HttpContext.Session.GetString("Usuario");
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                // Insertamos en la bitácora el ingreso a la vista de departamentos empresa
                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Vista Departamentos Empresa",
                    Descripcion = $"Ingreso a la vista de departamentos empresa por {usuario}",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                // Devolvemos la vista con el modelo
                return View(departamentosEmpresa);

            }
            catch (Exception e)
            {

                // En caso de error, volvemos a extraer el nombre de usuario de la sesión
                var usuario = HttpContext.Session.GetString("Usuario");

                // Insertamos en el log el error del proceso de login
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Vista Departamentos Empresa",
                    Descripcion = $"Error en el ingreso a la vista de departamentos empresa del {usuario}: {e.Message}.",
                    Estado = false
                });

                // En caso de error, mostramos un mensaje de error genérico al usuario
                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View(new List<DepartamentoEmpresaViewModel>());
            }

        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear()
        {
            try
            {
                // Obtener listas de departamentos y empresas activas
                var departamentos = await _daoDepartamento.ObtenerDepartamentosAsync();
                var empresas = await _daoEmpresa.ObtenerEmpresasAsync();

                // Preparamos el modelo para la vista
                var model = new DepartamentosEmpresaFormViewModel
                {
                    // Asignamos la lista de departamentos para el dropdown múltiple
                    Departamentos = departamentos?.Select(d => new SelectListItem
                    {
                        Value = d.IdDepartamento.ToString(),
                        Text = d.Nombre
                    }).ToList() ?? new List<SelectListItem>(),

                    // Asignamos la lista de empresas para el dropdown
                    Empresas = empresas?.Select(e => new SelectListItem
                    {
                        Value = e.IdEmpresa.ToString(),
                        Text = e.Nombre
                    }).ToList() ?? new List<SelectListItem>()
                };

                if (model.Departamentos.Count == 0 || model.Empresas.Count == 0)
                {
                    TempData["ErrorMessage"] = "No hay departamentos o empresas disponibles para asignar.";
                    return View(new DepartamentosEmpresaFormViewModel());
                }

                await RegistrarBitacora("Vista Crear Relación Departamento-Empresa",
                    "Acceso exitoso a la vista de creación de relación Departamento-Empresa");
                return View(model);
            }
            catch (Exception ex)
            {
                await RegistrarError("Cargar formulario de creación Departamento-Empresa", ex);
                TempData["ErrorMessage"] = "Error al cargar el formulario de creación.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear(DepartamentosEmpresaFormViewModel model)
        {
            try
            {
                if (model.FK_IdsDepartamentos == null || !model.FK_IdsDepartamentos.Any())
                {
                    ModelState.AddModelError("FK_IdsDepartamentos", "Debe seleccionar al menos un departamento");
                    ViewBag.ErrorMessage = "Debe seleccionar al menos un departamento"; // AÑADIDO: Para que SweetAlert lo capture
                }

                if (ModelState.IsValid)
                {
                    // Obtener relaciones existentes para la empresa seleccionada
                    var relacionesExistentes = await _daoDepartamentosEmpresa.ObtenerDepartamentosPorEmpresaAsync(model.FK_IdEmpresa);
                    var departamentosExistentes = relacionesExistentes.Select(r => r.FK_IdDepartamento).ToList();

                    // Filtrar departamentos que ya están asignados
                    var departamentosRepetidos = model.FK_IdsDepartamentos
                        .Where(id => departamentosExistentes.Contains(id))
                        .ToList();

                    // Si hay departamentos que ya están asignados, mostramos un error
                    if (departamentosRepetidos.Any())
                    {

                        // Cargar nuevamente los departamentos y empresas para mostrar en la vista
                        var departamentos = await _daoDepartamento.ObtenerDepartamentosAsync();

                        // Validar que existan departamentos
                        if (departamentos == null || !departamentos.Any())
                        {
                            ModelState.AddModelError("", "No hay departamentos disponibles.");
                            ViewBag.ErrorMessage = "No hay departamentos disponibles."; // AÑADIDO
                            return View(model);
                        }

                        // Filtrar los nombres de los departamentos repetidos
                        var nombresDepartamentos = departamentos
                            .Where(d => departamentosRepetidos.Contains(d.IdDepartamento))
                            .Select(d => d.Nombre);

                        // Cargar las empresas
                        var empresas = await _daoEmpresa.ObtenerEmpresasAsync();

                        // Validar que existan empresas
                        if (empresas == null || !empresas.Any())
                        {
                            ModelState.AddModelError("", "No hay empresas disponibles.");
                            ViewBag.ErrorMessage = "No hay empresas disponibles."; // AÑADIDO
                            return View(model);
                        }

                        // Asignar las listas de departamentos y empresas al modelo
                        model.Departamentos = departamentos.Select(d => new SelectListItem
                        {
                            Value = d.IdDepartamento.ToString(),
                            Text = d.Nombre
                        }).ToList();

                        // Asignar las empresas al modelo
                        model.Empresas = empresas.Select(e => new SelectListItem
                        {
                            Value = e.IdEmpresa.ToString(),
                            Text = e.Nombre
                        }).ToList();

                        // Crear un mensaje de error con los nombres de los departamentos repetidos
                        var mensajeError = $"Los siguientes departamentos ya están asignados a esta empresa: {string.Join(", ", nombresDepartamentos)}";

                        // Agregar el mensaje de error al modelo
                        ViewBag.ErrorMessage = mensajeError;

                        return View(model);
                    }
                    else
                    {
                        // Crear las relaciones departamento-empresa solo para los que no existen
                        foreach (var idDepartamento in model.FK_IdsDepartamentos)
                        {
                            var relacion = new DepartamentoEmpresaViewModel
                            {
                                FK_IdDepartamento = idDepartamento,
                                FK_IdEmpresa = model.FK_IdEmpresa
                            };

                            await _daoDepartamentosEmpresa.InsertarDepartamentoEmpresaAsync(relacion);
                        }

                        await RegistrarBitacora("Crear Relación Departamento-Empresa",
                            $"Se crearon {model.FK_IdsDepartamentos.Count} relaciones departamento-empresa");

                        TempData["SuccessMessage"] = "¡Relaciones creadas exitosamente!"; // CAMBIADO: Usar TempData
                        return RedirectToAction("Index");
                    }
                }

                // Recargar listas si hay error de validación
                var departamentosModel = await _daoDepartamento.ObtenerDepartamentosAsync();
                var empresasModel = await _daoEmpresa.ObtenerEmpresasAsync();

                model.Departamentos = departamentosModel?.Select(d => new SelectListItem
                {
                    Value = d.IdDepartamento.ToString(),
                    Text = d.Nombre
                }).ToList() ?? new List<SelectListItem>();

                model.Empresas = empresasModel?.Select(e => new SelectListItem
                {
                    Value = e.IdEmpresa.ToString(),
                    Text = e.Nombre
                }).ToList() ?? new List<SelectListItem>();

                // Si hay un mensaje de error en TempData, asegurarse de que se muestre
                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString(); // AÑADIDO: Pasar TempData a ViewBag si existe
                }

                return View(model);
            }
            catch (Exception ex)
            {
                await RegistrarError("Crear Relación Departamento-Empresa", ex);
                ModelState.AddModelError("", "Error al crear las relaciones. Por favor intente nuevamente.");
                ViewBag.ErrorMessage = "Error al crear las relaciones. Por favor intente nuevamente."; // AÑADIDO: Para que SweetAlert lo capture

                // Recargar listas en caso de error
                var departamentos = await _daoDepartamento.ObtenerDepartamentosAsync();
                var empresas = await _daoEmpresa.ObtenerEmpresasAsync();

                model.Departamentos = departamentos?.Select(d => new SelectListItem
                {
                    Value = d.IdDepartamento.ToString(),
                    Text = d.Nombre
                }).ToList() ?? new List<SelectListItem>();

                model.Empresas = empresas?.Select(e => new SelectListItem
                {
                    Value = e.IdEmpresa.ToString(),
                    Text = e.Nombre
                }).ToList() ?? new List<SelectListItem>();

                return View(model);
            }
        }
    }
}