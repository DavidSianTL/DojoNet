using System.Threading.Tasks;
using ExamenUno.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamenUno.Controllers
{
	public class CalculatorController : Controller
	{
		private readonly ICalculatorService _calculatorService;
		private readonly ISessionService _sessionService;
		private readonly ILogger<CalculatorController> _logger;
        public CalculatorController (ILogger<CalculatorController> logger,ICalculatorService calculatorService, ISessionService sessionService) 
		{
			_calculatorService = calculatorService; _sessionService = sessionService; _logger = logger;
		}

		[HttpGet]
		public IActionResult Index()
		{
            var redirect = _sessionService.validateSession(HttpContext);
            if (redirect != null) return redirect;

            _logger.LogInformation($"El usuario: {HttpContext.Session.GetString("User")} accedió a la pagina Index de la api soap calculator a la hora {DateTime.Now}");
            return View();
		}



		[HttpGet]
		public IActionResult Add()
		{
            _logger.LogInformation($"El usuario: {HttpContext.Session.GetString("User")} accedió a la pagina Add de la api soap calculator a la hora {DateTime.Now}");
            return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(int a, int b)
		{
			try
			{

                var redirect = _sessionService.validateSession(HttpContext);
                if (redirect != null) return redirect;


                var response = await _calculatorService.Sumar(a, b);
				if (response == 0) return View();

                _logger.LogInformation($"El usuario: {HttpContext.Session.GetString("User")} operó {a} + {b} y recibió {response}	a la Hora {DateTime.Now}");
				ViewBag.result = response;
				return View();

			} catch (Exception ex) {

				LoggerService.LogError(ex);
				return View();
			}

		}




		[HttpGet]
		public IActionResult Substract()
		{
            var redirect = _sessionService.validateSession(HttpContext);
            if (redirect != null) return redirect;

            _logger.LogInformation($"El usuario: {HttpContext.Session.GetString("User")} accedió a la pagina Substract de la api soap calculator a la hora {DateTime.Now}");
            return View();
		}

		[HttpPost]
		public async Task<IActionResult> Substract(int a, int b)
		{
			try
			{
				var response = await _calculatorService.Restar(a, b);
				if (response == 0) return View();

                _logger.LogInformation($"El usuario: {HttpContext.Session.GetString("User")} operó {a} - {b} y recibió {response} a la Hora {DateTime.Now}");

                ViewBag.result = response;
                return View();

            }catch (Exception ex)
			{
				LoggerService.LogError(ex);
				return View();
			}
		}



		[HttpGet]
		public IActionResult Multiply() 
		{

            var redirect = _sessionService.validateSession(HttpContext);
            if (redirect != null) return redirect;

            _logger.LogInformation($"El usuario: {HttpContext.Session.GetString("User")} accedió a la pagina Multiply de la api soap calculator a la hora {DateTime.Now}");
            return View();
		}

		[HttpPost]
		public async Task<IActionResult> Multiply(int a, int b)
		{
			try
			{
                var response = await _calculatorService.Multiplicar(a, b);
                if (response == 0) return View();

                _logger.LogInformation($"El usuario: {HttpContext.Session.GetString("User")} operó {a} * {b} y recibió {response} a la Hora {DateTime.Now}");

                ViewBag.result = response;
                return View();
            }
			catch (Exception ex)
			{

				LoggerService.LogError(ex);
				return View();
			}
		}



		[HttpGet]
		public IActionResult Divide(int response)
		{

            var redirect = _sessionService.validateSession(HttpContext);
            if (redirect != null) return redirect;

            _logger.LogInformation($"El usuario: {HttpContext.Session.GetString("User")} accedió a la pagina Divide de la api soap calculator a la hora {DateTime.Now}");

            return View();
		}

		[HttpPost]
        public async Task<IActionResult>Divide(int a, int b)
        {
			try
			{
                var response = await _calculatorService.Dividir(a, b);
                if (response == 0) return View();

                _logger.LogInformation($"El usuario: {HttpContext.Session.GetString("User")} operó {a} / {b}  y recibió {response} a la Hora {DateTime.Now}");

                ViewBag.result = response;
                return View();
            }
			catch (Exception ex)
			{
				LoggerService.LogError(ex);
				return View();
			}
        }
    }
}
