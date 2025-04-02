using Microsoft.AspNetCore.Mvc;

namespace CRUD_Usuarios_simple.Controllers
{
    public class VerController : Controller
    {
        public IActionResult Ver()
        {
            return View();
        }
    }
}
