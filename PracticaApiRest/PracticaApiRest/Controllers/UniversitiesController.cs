using Microsoft.AspNetCore.Mvc;
using PracticaApiRest.Models;
using PracticaApiRest.Service;
using System.Threading.Tasks;

namespace PracticaApiRest.Controllers
{
    public class UniversitiesController : Controller
    {
        private readonly UniversitiesService _service= new UniversitiesService();

        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Ver(string name)
        {
            var universities = await _service.GetUniversityByNameAsync(name);
            return View(universities);
        }
        public ActionResult Crear() => View();

        [HttpPost]
        public async Task<ActionResult> Crear(UniversitiesModel universities)
        {
            await _service.CreateUniversityAsync(universities);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Editar(string name)
        {
            var universities = await _service.GetUniversityByNameAsync(name);
            return View(universities);
        }

        [HttpPost]
        public async Task<ActionResult> Editar(string name, UniversitiesModel universities)
        {
            await _service.UpdateUniversityAsync(name, universities);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Eliminar(string name)
        {
            var universities = await _service.GetUniversityByNameAsync(name);
            return View(universities);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> EliminadoCorrectamente(string name)
        {
            await _service.DeleteUniversityAsync(name);
            return RedirectToAction("Index");
        }
    }
}
