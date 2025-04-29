using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _Evaluacion_Mensual_Abril.Models;
using System.IO;

namespace _Evaluacion_Mensual_Abril.Controllers
{

    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            var userNombre = HttpContext.Session.GetString("UsrNombre");
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");
            var mostrarAlerta = HttpContext.Session.GetString("MostrarAlerta");

            try
            {
                if (userNombre != null)
                {
                    ViewBag.usrNombre = userNombre;
                    ViewBag.NombreCompleto = nombreCompleto;

                    var isLoggedIn = HttpContext.Session.GetString("UsrNombre") != null;
                    ViewData["isLoggedIn"] = isLoggedIn;

                    // Solo mostrar la alerta si "MostrarAlerta" está configurado
                    if (mostrarAlerta == "true")
                    {
                        ViewBag.MostrarAlerta = true;
                        HttpContext.Session.SetString("MostrarAlerta", "false"); // Desactivar la alerta
                    }
                    else
                    {
                        ViewBag.MostrarAlerta = false;
                    }

                    return View();
                }

                else
                {
                    // File para poder crear un archivo que contenga el log
                    var mensaje = " Error: No se pudo accedaer a la vista de inicio, posible error en el Login";

                    System.IO.File.AppendAllText("log.txt", DateTime.Now + mensaje + Environment.NewLine);

                    return RedirectToAction("Login", "Login");
                }
            }
            catch(Exception e)
            {
                // Manejo de excepciones
                var mensaje = " Error: No se pudo accedaer a la vista de inicio, posible error en el Login" + e.Message;
                System.IO.File.AppendAllText("log.txt", DateTime.Now + mensaje + Environment.NewLine);
                return RedirectToAction("Login", "Login");
            }

            

        }

    }

}
