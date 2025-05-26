using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaSeguridadMVC.Controllers
{
    public class SistemaWSController : Controller
    {
        // GET: SistemaWSController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SistemaWSController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SistemaWSController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SistemaWSController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SistemaWSController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SistemaWSController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SistemaWSController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SistemaWSController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
