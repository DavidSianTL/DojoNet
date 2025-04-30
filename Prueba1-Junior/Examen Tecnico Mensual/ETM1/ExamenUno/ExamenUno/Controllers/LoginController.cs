using System.Text.Json;
using ExamenUno.Models;
using ExamenUno.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamenUno.Controllers
{
	public class LoginController : Controller
	{
		private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger) { _logger = logger;  }


		[HttpGet]
        public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Login(Usuario user)
		{
			//Validaciones de modelo y campos
			if (!ModelState.IsValid)
			{
				return View();
			}
			if(string.IsNullOrEmpty(user.username) || string.IsNullOrEmpty(user.password))
			{
				return View();
			}




			try
			{
				var usersRoute = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Usuarios.json");//ruta del archivo JSON con los usuarios
																										//se valida si el archivo existe en la ruta
				if (!System.IO.File.Exists(usersRoute))
				{
					ModelState.AddModelError(string.Empty, "Error al iniciar sesión, intentalo más tarde.");
					return View();
				}


				var content = System.IO.File.ReadAllText(usersRoute);//leemos
				var users = JsonSerializer.Deserialize<List<Usuario>>(content);//guardamos los usuarios en una lista 

				//buscamos un usuario que conincida con el ingresado
				var validUser = users.FirstOrDefault(u =>
					u.username == user.username &&
					u.password == user.password
				);

				//si el usuario es valido redirigimos a home/index
				if (validUser != null)
				{
					HttpContext.Session.SetString("User", validUser.username);
                    _logger.LogInformation($"Sessión iniciada por el usuario {HttpContext.Session.GetString("User") ?? user.username}, en la hora: {DateTime.Now}");
                    return RedirectToAction("Index", "Home");
				}
				else
				{
                    _logger.LogWarning($"Intento de inicio de sesión por el usuario {user.username}, en la hora: {DateTime.Now}");
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
					return View();
				}


			}catch (Exception ex){

				_logger.LogCritical($"Error al intentar iniciar sesión por el usuario {user.username}, en la hora: {DateTime.Now} {ex.Message}");
				LoggerService.LogError(ex);
				ModelState.AddModelError(string.Empty, "Error al iniciar sesión intentalo más tarde.");
				return View();

			}
		}


		public IActionResult Logout()
		{
			_logger.LogInformation($"El usuario {HttpContext.Session.GetString("User")} a cerrado la sesión");
			HttpContext.Session.Clear();
			return RedirectToAction("Login", "Login");
		}
	}
}
