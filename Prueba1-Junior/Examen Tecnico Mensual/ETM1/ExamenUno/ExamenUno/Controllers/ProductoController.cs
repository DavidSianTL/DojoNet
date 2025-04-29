using ExamenUno.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamenUno.Controllers
{
	public class ProductoController : Controller
	{
		private readonly ISessionService _sessionService;
        public ProductoController( ISessionService sessionService)
		{
			_sessionService = sessionService;
        }




		[HttpGet]
		public IActionResult ShowProd()
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;

			return View();
		}

		[HttpGet]
		public IActionResult CreateProd()
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;

			return View();
		}

		[HttpGet]
		public IActionResult EditProd()
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;

			return View();
		}

		[HttpGet]
		public IActionResult DeleteProd()
		{
            var redirect = _sessionService.validateSession(HttpContext);
            if (redirect != null) return redirect;

            return View();
		}

		

	}
}
