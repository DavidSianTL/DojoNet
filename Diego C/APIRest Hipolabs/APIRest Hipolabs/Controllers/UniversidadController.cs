using APIRest_Hipolabs.Models;
using APIRest_Hipolabs.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIRest_Hipolabs.Controllers
{
    public class UniversidadController : Controller
    {
        private readonly UniversidadService _universidadService;

        public UniversidadController(UniversidadService universidadService)
        {
            _universidadService = universidadService;
        }

        
        [HttpPost]
        public async Task<IActionResult> BuscarUniversidades(string pais)
        {
            var universidades = await _universidadService.ObtenerUniversidadesPorPais(pais);
            return View(universidades);
        }
    }
}
