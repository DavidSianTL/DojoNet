using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using pruebas.Models;

namespace pruebas.Controllers

{
    public class HomeController : Controller
    {
       

        public IActionResult Index()
        {
            ViewBag.NombreCompleto = HttpContext.Session.GetString("NombreCompleto");
            return View();
        }

        
    }
}
