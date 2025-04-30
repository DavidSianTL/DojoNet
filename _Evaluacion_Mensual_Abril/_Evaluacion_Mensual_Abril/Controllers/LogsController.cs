using Microsoft.AspNetCore.Mvc;
using System.IO;
using _Evaluacion_Mensual_Abril.Services;

namespace _Evaluacion_Mensual_Abril.Controllers
{
    public class LogsController : Controller
    {
        private readonly LoggerServices _loggerServices;

        public LogsController()
        {
            _loggerServices = new LoggerServices();
        }


        public IActionResult VerLogs()
        {
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "logs.txt");

            if (!System.IO.File.Exists(logFilePath))
            {
                ViewData["ErrorMessage"] = "No hay registros disponibles.";
                return View();
            }

            var logs = System.IO.File.ReadAllLines(logFilePath);
            return View(logs);
        }
    }
}
