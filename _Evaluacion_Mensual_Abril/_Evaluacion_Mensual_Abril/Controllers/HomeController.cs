using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using _Evaluacion_Mensual_Abril.Models;
using _Evaluacion_Mensual_Abril.Services;
using System.IO;
using System.Linq;

namespace _Evaluacion_Mensual_Abril.Controllers
{
    public class HomeController : Controller
    {
        private readonly LoggerServices _loggerServices;

        public HomeController()
        {
            _loggerServices = new LoggerServices();
        }

        private string ObtenerUsuario()
        {
            return HttpContext.Session.GetString("NombreCompleto") ?? "Anónimo";
        }

        private string FechaHoraActual()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public IActionResult Index()
        {
            var nombre = HttpContext.Session.GetString("NombreCompleto");
            if (string.IsNullOrEmpty(nombre))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.NombreCompleto = nombre;
            _loggerServices.RegistrarAccion(nombre, $"Accedió al Home/Index - {FechaHoraActual()}");

            return View();
        }

        public IActionResult Privacy()
        {
            var nombre = HttpContext.Session.GetString("NombreCompleto");
            if (string.IsNullOrEmpty(nombre))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.NombreCompleto = nombre;
            _loggerServices.RegistrarAccion(nombre, $"Accedió a la vista de privacidad - {FechaHoraActual()}");

            return View();
        }

        public IActionResult Error(int? statusCode)
        {
            string usuario = ObtenerUsuario();
            string mensaje = statusCode == 404
                ? "Página no encontrada (404)"
                : $"Error inesperado (Código: {statusCode})";

            _loggerServices.RegistrarError(usuario, $"{mensaje} - {FechaHoraActual()}");

            ViewData["ErrorMessage"] = "Ocurrió un error inesperado.";
            return View("Error");
        }

        public IActionResult Logs(int page = 1, int pageSize = 20)
        {
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "logs.txt");

            if (!System.IO.File.Exists(logFilePath))
            {
                ViewData["ErrorMessage"] = "No hay registros disponibles.";
                return View(Array.Empty<string>());
            }

            var allLogs = System.IO.File.ReadAllLines(logFilePath).Reverse().ToList();
            var totalLogs = allLogs.Count;
            var totalPages = (int)Math.Ceiling(totalLogs / (double)pageSize);

            page = Math.Max(1, Math.Min(page, totalPages));

            var logsToShow = allLogs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;

            return View(logsToShow);
        }
    }
}
