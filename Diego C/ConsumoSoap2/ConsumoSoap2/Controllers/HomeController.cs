using System.Diagnostics;
using System.Threading.Tasks;
using ConsumoSoap2.Models;
using ConsumoSoap2.Servicio;
using Microsoft.AspNetCore.Mvc;

namespace ConsumoSoap2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConsumoDeMetodo _consumoDeMetodos;

        public HomeController(ILogger<HomeController> logger, IConsumoDeMetodo ConsumoDeMetodos)
        {
            _logger = logger;
            _consumoDeMetodos = ConsumoDeMetodos;
        }

        public async Task<IActionResult> Index()
        {
            // llamada al servicio
            var resultado = string.Empty;
            try
            {
                resultado = await _consumoDeMetodos.ConvertNumerosALetras();
                ViewBag.Result = resultado;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
