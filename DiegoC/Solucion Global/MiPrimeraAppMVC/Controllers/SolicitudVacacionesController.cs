using Microsoft.AspNetCore.Mvc;
using MiPrimeraAppMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace MiPrimeraAppMVC.Controllers
{
    public class SolicitudVacacionesController : Controller
    {
        //Lista que simula la base de datos
        private static List<SolicitudVacacionesModel> _solicitudes = new List<SolicitudVacacionesModel>();

        //Metodo que muestra las solicitudes
        public IActionResult Index()
        {
            return View(_solicitudes);
        }
        //Metodo que crea una solicitud
        public IActionResult Crear()
        {
            return View();
        }

        //Metodo que guarda la solicitud
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(SolicitudVacacionesModel solicitud) //Recibe un objeto de tipo SolicitudVacacionesModel
        {
            if (ModelState.IsValid)//Valida si el modelo es valido midiendo las anotaciones
            {
                _solicitudes.Add(solicitud);//Agrega la solicitud a la lista
                return RedirectToAction("Index");//Redirecciona a la vista Index
            }
            return View(solicitud);//Regresa la vista con la solicitud si no es valida
        }
       

        }

}
