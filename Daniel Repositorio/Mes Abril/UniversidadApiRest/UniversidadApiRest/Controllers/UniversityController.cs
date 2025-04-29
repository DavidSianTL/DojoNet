using Microsoft.AspNetCore.Mvc;
using UniversidadApiRest.Services;
using UniversidadApiRest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversidadApiRest.Controllers
{
    public class UniversitiesController : Controller
    {
        private readonly UniversityService _service = new UniversityService();
        public async Task<IActionResult> Index(string country)
        {
            List<University> universities = new List<University>();

            if (!string.IsNullOrEmpty(country))
            {
                universities = await _service.GetUniversitiesByCountryAsync(country);
            }
            ViewBag.Country = country;

            return View(universities);
        }
    }
}
