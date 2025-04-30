using Microsoft.AspNetCore.Mvc;
using Final_Evaluacion_Mensual_Abril.Services;
using System;
using Final_Evaluacion_Mensual_Abril.Filters;
using Proyecto1.Services;

namespace Final_Evaluacion_Mensual_Abril.Controllers
{
    [LoginAuthorize]
    public class BitacoraController : Controller
    {
        private readonly BitacoraService _bitacora;

        public BitacoraController(BitacoraService bitacora)
        {
            _bitacora = bitacora;
        }

        public IActionResult Index(DateTime? desde, DateTime? hasta, string usuario)
        {
            // Establecer valores por defecto (últimos 7 días)
            desde = desde ?? DateTime.Now.AddDays(-7);
            hasta = hasta ?? DateTime.Now;

            var logs = _bitacora.FiltrarLogs(desde, hasta, usuario);

            ViewBag.Desde = desde.Value.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta.Value.ToString("yyyy-MM-dd");
            ViewBag.UsuarioFiltro = usuario;

            return View(logs);
        }

        [HttpPost]
        public IActionResult Exportar(DateTime desde, DateTime hasta, string usuario)
        {
            var logs = _bitacora.FiltrarLogs(desde, hasta, usuario);

            var csv = "Fecha,Usuario,Acción,Controlador,Descripción,EsError\n";
            csv += string.Join("\n", logs.Select(l =>
                $"\"{l.Fecha}\",\"{l.Usuario}\",\"{l.Accion}\",\"{l.Controlador}\",\"{l.Descripcion}\",\"{(l.EsError ? "Sí" : "No")}\""));

            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", $"bitacora_{DateTime.Now:yyyyMMddHHmmss}.csv");
        }
    }
}