using APIRESTUniversidades.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIRESTUniversidades.Controllers
{
    public class UniversityController : Controller
    {
        private readonly UniversityService _service;

        public UniversityController()
        {
            _service = new UniversityService();
        }


        public async Task<IActionResult> UniversidadPorPais(string nombrePais)
        {
            if (string.IsNullOrEmpty(nombrePais))
            {
                nombrePais = "unknow";
            }

            var universities = await _service.UniversidadPorPais(nombrePais);
            return View(universities);
        }
        
        public async Task<IActionResult> UniversidadPorNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                nombre = "Mariano";
            }

            var universidades = await _service.UniversidadPorNombre(nombre);
            return View(universidades);
        }
    }
}
