using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class BitacorasController : Controller
    {

        // GET: Bitacoras
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Bitacoras/Details


    }
}
