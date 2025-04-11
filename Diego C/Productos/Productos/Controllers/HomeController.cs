using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Productos.Models;

namespace Productos.Controllers
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
       

    }
    }
