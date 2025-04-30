using System.Threading.Tasks;
using ExamenUno.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamenUno.Controllers
{
	public class CalculatorController : Controller
	{
		private readonly ICalculatorService _calculatorService;
		private readonly ISessionService _sessionService;

        public CalculatorController (ICalculatorService calculatorService, ISessionService sessionService) { _calculatorService = calculatorService; _sessionService = sessionService; }

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}



		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(int a, int b)
		{
			try
			{

				var response = await _calculatorService.Sumar(a, b);
				if (response == 0) return View();

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
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Substract(int a, int b)
		{
			try
			{
				var response = await _calculatorService.Restar(a, b);
				if (response == 0) return View();

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
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Multiply(int a, int b)
		{
			try
			{
                var response = await _calculatorService.Multiplicar(a, b);
                if (response == 0) return View();

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
			return View();
		}

		[HttpPost]
        public async Task<IActionResult>Divide(int a, int b)
        {
			try
			{
                var response = await _calculatorService.Dividir(a, b);
                if (response == 0) return View();

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
