using System.Diagnostics;
using ClinicaMedicaAPIREST.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedicaAPIREST.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       
       
    }
}
