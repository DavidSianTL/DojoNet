using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class DepartamentoController : Controller
    {
        private readonly daoDepartamentoWSAsync _daoDepartamento;
        private readonly daoBitacoraWSAsync _daoBitacoraWS;
        private readonly daoLogWSAsync _daoLog;

        public DepartamentoController()
        {
            string _connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _daoDepartamento = new daoDepartamentoWSAsync(_connectionString);
            _daoBitacoraWS = new daoBitacoraWSAsync(_connectionString);
            _daoLog = new daoLogWSAsync(_connectionString);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var departamentos = await _daoDepartamento.ObtenerDepartamentosAsync();

                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuario = HttpContext.Session.GetString("Usuario");
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                await _daoBitacoraWS.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Vista Departamentos",
                    Descripcion = $"Ingreso a la vista de departamentos exitoso por {usuario}.",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                return View(departamentos);
            }
            catch (Exception ex)
            {
                var usuario = HttpContext.Session.GetString("Usuario");

                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Vista Departamentos",
                    Descripcion = $"Error en vista departamentos por {usuario}: {ex.Message}",
                    Estado = false
                });

                return View("Error");
            }
        }

        [AuthorizeRole("SuperAdmin", "Admin")]
        [HttpGet]
        public IActionResult AgregarDepartamento()
        {
            return View("Agregar", "Departamento");
        }

        [AuthorizeRole("SuperAdmin")]
        [HttpGet]
        public IActionResult EditarDepartamento()
        {
            return View("Editar", "Departamento");
        }

        [HttpPost]
        public IActionResult EliminarDepartamento(int Id)
        {
            // Lógica para eliminar departamento
            return RedirectToAction("Index");
        }
    }
}