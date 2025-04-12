using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Filters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Proyecto.Utils; // <-- Importar Logger

namespace Proyecto.Controllers
{
    [RequireLogin]
    public class DashboardController : Controller
    {
        private readonly string _jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "productos.json");
        private const string SessionUserId = "UsuarioId";

        public IActionResult Index()
        {
            var productos = GetProductos();

            var usuarioId = HttpContext.Session.GetString(SessionUserId);
            if (!string.IsNullOrEmpty(usuarioId))
            {
                Logger.RegistrarAccion(usuarioId, "Accedió al dashboard");
            }

            if (productos == null || !productos.Any())
            {
                TempData["ErrorMessage"] = "No hay productos disponibles.";
            }

            return View(productos);
        }

        private List<Producto> GetProductos()
        {
            if (!System.IO.File.Exists(_jsonPath))
            {
                System.IO.File.WriteAllText(_jsonPath, "[]");
                return new List<Producto>();
            }

            string json = System.IO.File.ReadAllText(_jsonPath);
            return JsonConvert.DeserializeObject<List<Producto>>(json) ?? new List<Producto>();
        }
    }
}
