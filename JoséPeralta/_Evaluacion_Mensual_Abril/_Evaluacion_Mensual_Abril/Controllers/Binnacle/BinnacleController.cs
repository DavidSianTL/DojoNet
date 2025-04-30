using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using _Evaluacion_Mensual_Abril.Models.Binnacle;

namespace _Evaluacion_Mensual_Abril.Controllers
{
    public class BinnacleController : Controller
    {
        private readonly string _rutaLog = "log.txt";

        public async Task<IActionResult> Binnacle()
        {
            var logs = new List<BinnacleViewModel>();

            if (System.IO.File.Exists(_rutaLog))
            {
                var lineas = System.IO.File.ReadAllLines(_rutaLog);
                var regex = new Regex(@"^(?<fecha>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}) \[Usuario: (?<usuario>.*?)\] \[Acción: (?<accion>.*?)\] (?<descripcion>.*)$");

                foreach (var linea in lineas)
                {
                    var match = regex.Match(linea);
                    if (match.Success)
                    {
                        logs.Add(new BinnacleViewModel
                        {
                            FechaHora = match.Groups["fecha"].Value,
                            Usuario = match.Groups["usuario"].Value,
                            Accion = match.Groups["accion"].Value,
                            Descripcion = match.Groups["descripcion"].Value
                        });
                    }
                }
            }

            var modelo = new BinnaclePageViewModel
            {
                Logs = logs
            };

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(BinnaclePageViewModel model)
        {
            var entrada = model.NuevaEntrada;
            var usuario = HttpContext.Session.GetString("NombreCompleto") ?? "No identificado";
            var nuevaLinea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [Usuario: {usuario}] [Acción: {entrada.NuevaAccion}] {entrada.NuevaDescripcion}{Environment.NewLine}";
            System.IO.File.AppendAllText(_rutaLog, nuevaLinea);

            return RedirectToAction("Binnacle");
        }

    }
}
