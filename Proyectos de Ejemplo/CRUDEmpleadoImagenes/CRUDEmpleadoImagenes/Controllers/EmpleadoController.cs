
using System.Web;
using CRUDEmpleadoImagenes.Models;
using CRUDEmpleadoImagenes.DAO;
using System.Configuration;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CRUDEmpleadoImagenes.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly DAOEmpleado dao;
        public EmpleadoController(DAOEmpleado daoEmpleado)
        {
            dao = daoEmpleado;
        }
        public IActionResult Index()
        {
            var empleados = dao.ObtenerTodos();
            return View(empleados);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Empleado empleado, IFormFile FotoArchivo, IFormFile FirmaArchivo, IFormFile PDFArchivo)
        {

            // Eliminamos errores generados 
            ModelState.Remove("Foto");
            ModelState.Remove("Firma");
            ModelState.Remove("DocumentoPDF");
            const long maxFileSize = 500 * 1024; // 500 KB

            // Validación manual de archivos
            if (FotoArchivo == null || FotoArchivo.Length == 0)
                ModelState.AddModelError("FotoArchivo", "Debe subir una foto");
            else if (!FotoArchivo.ContentType.StartsWith("image"))
                ModelState.AddModelError("FotoArchivo", "La foto debe ser una imagen.");
            else if (FotoArchivo.Length > maxFileSize)
                ModelState.AddModelError("FotoArchivo", "La foto no debe superar los 500 KB.");



            if (FirmaArchivo == null || FirmaArchivo.Length == 0)
                ModelState.AddModelError("FirmaArchivo", "Debe subir una firma");
            else if (!FirmaArchivo.ContentType.StartsWith("image"))
                ModelState.AddModelError("FirmaArchivo", "La firma debe ser una imagen.");
            else if (FirmaArchivo.Length > maxFileSize)
                ModelState.AddModelError("FirmaArchivo", "La firma no debe superar los 500 KB.");



            if (PDFArchivo == null || PDFArchivo.Length == 0)
                ModelState.AddModelError("PDFArchivo", "Debe subir un documento PDF");
            else if (PDFArchivo.ContentType != "application/pdf")
                ModelState.AddModelError("PDFArchivo", "Debe subir un archivo PDF válido.");
            else if (PDFArchivo.Length > (2 * 1024 * 1024)) // 2 MB por ejemplo
                ModelState.AddModelError("PDFArchivo", "El archivo PDF no debe superar los 2 MB.");



            // Convertir archivos a byte[]
            if (ModelState.IsValid)
            {
                if (FotoArchivo != null)
                {
                    using var ms = new MemoryStream();
                    await FotoArchivo.CopyToAsync(ms);
                    empleado.Foto = ms.ToArray();
                }

                if (FirmaArchivo != null)
                {
                    using var ms = new MemoryStream();
                    await FirmaArchivo.CopyToAsync(ms);
                    empleado.Firma = ms.ToArray();
                }

                if (PDFArchivo != null)
                {
                    using var ms = new MemoryStream();
                    await PDFArchivo.CopyToAsync(ms);
                    empleado.DocumentoPDF = ms.ToArray();
                }

                dao.Insertar(empleado); // Guardar en DB
                return RedirectToAction("Index");
            }

            return View(empleado);


           
        }

        public IActionResult Edit(int id)
        {
            var empleado = dao.ObtenerPorId(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Empleado empleado, IFormFile FotoArchivo, IFormFile FirmaArchivo, IFormFile PDFArchivo)
        {

            // Eliminamos errores generados 
            ModelState.Remove("Foto");
            ModelState.Remove("Firma");
            ModelState.Remove("DocumentoPDF");
            ModelState.Remove("FotoArchivo");
            ModelState.Remove("FirmaArchivo");
            ModelState.Remove("PDFArchivo");

            const long maxFileSize = 500 * 1024; // 500 KB

            var empleadoActual = dao.ObtenerPorId(empleado.EmpleadoId);

            if (empleadoActual == null)
                return NotFound();

            if (FotoArchivo != null && FotoArchivo.Length > 0)
            {
                if (!FotoArchivo.ContentType.StartsWith("image"))
                    ModelState.AddModelError("FotoArchivo", "La foto debe ser una imagen.");
                else if (FotoArchivo.Length > maxFileSize)
                    ModelState.AddModelError("FotoArchivo", "La foto no debe superar los 500 KB.");
               else { 
                    using var ms = new MemoryStream();
                    await FotoArchivo.CopyToAsync(ms);
                    empleado.Foto = ms.ToArray();
                 }
            }
            else
            {
                empleado.Foto = empleadoActual.Foto;
               
            }

            if (FirmaArchivo != null && FirmaArchivo.Length > 0)
            {
                if (!FirmaArchivo.ContentType.StartsWith("image"))
                    ModelState.AddModelError("FirmaArchivo", "La foto debe ser una imagen.");
                else if (FirmaArchivo.Length > maxFileSize)
                    ModelState.AddModelError("FirmaArchivo", "La foto no debe superar los 500 KB.");
               
                else
                {
                    using var ms = new MemoryStream();
                    await FirmaArchivo.CopyToAsync(ms);
                    empleado.Firma = ms.ToArray();
                }
            }
            else
            {
                empleado.Firma = empleadoActual.Firma;
            }

            if (PDFArchivo != null && PDFArchivo.Length > 0)
            {
                if (PDFArchivo.ContentType != "application/pdf")
                    ModelState.AddModelError("PDFArchivo", "Debe subir un archivo PDF válido.");
                else if (PDFArchivo.Length > (2 * 1024 * 1024)) // 2 MB por ejemplo
                    ModelState.AddModelError("PDFArchivo", "El archivo PDF no debe superar los 2 MB.");
                else
                {
                    using var ms = new MemoryStream();
                    await PDFArchivo.CopyToAsync(ms);
                    empleado.DocumentoPDF = ms.ToArray();
                }
            }
            else
            {
                empleado.DocumentoPDF = empleadoActual.DocumentoPDF;
            }

            // Convertir archivos a byte[]
            if (ModelState.IsValid)
            {
               

                dao.Actualizar(empleado); // Guardar en DB
                return RedirectToAction("Index");
            }

            return View(empleado);


        }

        public IActionResult Delete(int id)
        {
            dao.Eliminar(id);
            return RedirectToAction("Index");
        }

        public IActionResult VerArchivo(int id, string tipo)
        {
            var archivo = dao.ObtenerArchivo(id, tipo);
            if (archivo == null)
                return NotFound();

            string contentType = tipo switch
            {
                "foto" => "image/jpeg",
                "firma" => "image/jpeg",
                "pdf" => "application/pdf",
                _ => "application/octet-stream"
            };

            return File(archivo, contentType);
        }

                
        // Método auxiliar para convertir archivo a byte[]
        private async Task<byte[]> ConvertirArchivoABytes(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return null;

            using var ms = new MemoryStream();
            await archivo.CopyToAsync(ms);
            return ms.ToArray();
        }

    }
}
