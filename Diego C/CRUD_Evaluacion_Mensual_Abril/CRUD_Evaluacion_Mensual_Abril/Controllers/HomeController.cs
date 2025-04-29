using System.Diagnostics;
using CRUD_Evaluacion_Mensual_Abril.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Evaluacion_Mensual_Abril.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            //verificamos si el usuario esta logeado
            var usrNombre = HttpContext.Session.GetString("UsrNombre");
            var NombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (usrNombre == null)
            {
                return RedirectToAction("Login", "Login");


            }
            ViewBag.usrNombre = usrNombre;
            ViewBag.NombreCompleto = NombreCompleto;



            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

