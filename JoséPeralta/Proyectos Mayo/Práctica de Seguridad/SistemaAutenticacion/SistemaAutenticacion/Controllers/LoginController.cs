using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaAutenticacion.Controllers
{
    public class LoginController : Controller
    {
        // GET: Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


    }
}
