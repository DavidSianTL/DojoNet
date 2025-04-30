using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Crud.Models; // Espacio de nombres correcto
using AppFuturista.Services;

namespace AppFuturista.Controllers
{
    public class HomeController : Controller
    {
        private readonly BitacoraService _bitacoraService;

        public HomeController(BitacoraService bitacoraService)
        {
            _bitacoraService = bitacoraService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/Error/{statusCode:int}")]
        public IActionResult StatusCodeError(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return View("Error404");
                default:
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [Route("/Error")]
        public IActionResult Error(string mensaje = null)
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Mensaje = mensaje ?? "Ha ocurrido un error inesperado"
            });
        }

        public IActionResult AccesoDenegado()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error500()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
