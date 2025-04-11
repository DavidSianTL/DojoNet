using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Practica_JavaScript.Models;
using System.IO;
using Practica_JavaScript.Services;

namespace Practica_JavaScript.Controllers
{

    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            var userNombre = HttpContext.Session.GetString("UsrNombre");
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (userNombre != null)
            {
                ViewBag.usrNombre = userNombre;
                ViewBag.NombreCompleto = nombreCompleto;

                return View();
            }
            else
            {
                // File para poder crear un archivo que contenga el log
                var mensaje = " Error: No se pudo acceder a la vista de inicio, posible error en el Login";

                System.IO.File.AppendAllText("log.txt", DateTime.Now + mensaje + Environment.NewLine);

                return RedirectToAction("Login", "Login");
            }
        }

    }

}
