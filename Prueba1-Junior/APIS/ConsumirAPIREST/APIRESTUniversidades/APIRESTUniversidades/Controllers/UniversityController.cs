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
    }
}
