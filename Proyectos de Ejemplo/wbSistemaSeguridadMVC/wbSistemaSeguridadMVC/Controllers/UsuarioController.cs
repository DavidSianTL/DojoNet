using Microsoft.AspNetCore.Mvc;

using wbSistemaSeguridadMVC.Data;
using wbSistemaSeguridadMVC.Models;
using ClosedXML.Excel;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml.InkML;

namespace wbSistemaSeguridadMVC.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly daoUsuario _datos;

        public UsuarioController()
        {
            string connectionString = "Server=NEWPEGHOSTE\\SQLEXPRESS;Database=SistemaSeguridad;Integrated Security=True;TrustServerCertificate=True;";
            //string connectionString = config.GetConnectionString("ConexionSistemaSeguridad");
            _datos = new daoUsuario(connectionString);
        }

        public async Task<IActionResult> Index()
        {
            try 
            { 
                var usuarios = await _datos.ObtenerUsuarios();
                return View(usuarios);

             }
            catch (Exception ex)
            {
                return Content("Error al obtener los Usuarios: " + ex.Message);

            }

        }



        public async Task<ActionResult> Crear()
        {
            ViewBag.Estados = await _datos.ObtenerEstados();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Crear(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var contraseniaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenia);

                var nuevoUsuario = new Usuario
                {
                    UsuarioLg = usuario.UsuarioLg,
                    NomUsuario = usuario.NomUsuario,
                    Contrasenia = contraseniaHash,
                    FkIdEstado = 1 // activo
                };


                usuario.FechaCreacion = DateTime.Now;
                
                int creado = await _datos.InsertarUsuario(nuevoUsuario);

                if (creado == 1)
                {
                    ViewBag.Mensaje = "Usuario registrado correctamente.";
                    ModelState.Clear(); // limpia el formulario
                }
                else
                {
                    ViewBag.Mensaje = "Error al registrar el usuario.";
                }

                return RedirectToAction("Index");
            }

            ViewBag.Estados = _datos.ObtenerEstados();
            return View(usuario);
        }

        public async Task<ActionResult> Editar(int id)
        {
            var usuario = await _datos.ObtenerUsuarioPorId(id);
            if (usuario == null)
            {

                return NotFound();
            }
            ViewBag.Estados = _datos.ObtenerEstados();
            return View(usuario);
        }

        [HttpPost]
        public async Task<ActionResult> Editar(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    Console.WriteLine($"{item.Key} → {item.Value?.Errors.FirstOrDefault()?.ErrorMessage}");
                }


                ViewBag.Estados = await _datos.ObtenerEstados();
                return View(usuario);
            }

            int resultado = await _datos.ActualizarUsuario(usuario);
            if (resultado == 1)
            {
                return RedirectToAction("Index");
            }


            ViewBag.Estados = await _datos.ObtenerEstados();
            ModelState.AddModelError("", "Error al actualizar el usuario.");
            return View(usuario);
        }

        public async Task<ActionResult> Eliminar(int id)
        {
            var usuario = await _datos.ObtenerUsuarioPorId(id);
            return View(usuario);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<ActionResult> ConfirmarEliminar(int id)
        {
            await _datos.EliminarUsuario(id);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> ExportarUsuariosExcel()
        {
            try
            {
                var usuarios = await _datos.ObtenerUsuarios();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Usuarios");

                    // Encabezados
                    worksheet.Cell(1, 1).Value = "ID Usuario";
                    worksheet.Cell(1, 2).Value = "Login Usuario";
                    worksheet.Cell(1, 3).Value = "Nombre Completo";
                    worksheet.Cell(1, 4).Value = "Estado";
                    worksheet.Cell(1, 5).Value = "Fecha Creación";

                    int fila = 2;
                    foreach (var usuario in usuarios)
                    {
                        worksheet.Cell(fila, 1).Value = usuario.IdUsuario;
                        worksheet.Cell(fila, 2).Value = usuario.UsuarioLg;
                        worksheet.Cell(fila, 3).Value = usuario.NomUsuario;
                        worksheet.Cell(fila, 4).Value = usuario.Estado?.Descripcion ?? "N/A";
                        worksheet.Cell(fila, 5).Value = usuario.FechaCreacion.ToString("yyyy-MM-dd");
                        fila++;
                    }

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "ReporteUsuarios.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("Error al generar el reporte Excel: " + ex.Message);
            }
        }



    }
}
