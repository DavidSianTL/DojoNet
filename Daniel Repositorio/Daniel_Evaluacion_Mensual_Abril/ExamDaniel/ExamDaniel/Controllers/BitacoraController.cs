using Microsoft.AspNetCore.Mvc;
using ExamDaniel.bitacora;
using System.Collections.Generic;
using System.Linq;

namespace ExamDaniel.Controllers
{
    public class BitacoraController : Controller
    {
        public IActionResult Index(string filtro = "Todos")
        {
            var eventos = BitacoraManager.LeerEventos();

            // Filtrar si no es "Todos"
            if (!string.IsNullOrEmpty(filtro) && filtro != "Todos")
            {
                eventos = eventos.Where(e => e.Contains($"| {filtro} |")).ToList();
            }

            ViewBag.FiltroSeleccionado = filtro;
            return View(eventos);
        }
    }
}
