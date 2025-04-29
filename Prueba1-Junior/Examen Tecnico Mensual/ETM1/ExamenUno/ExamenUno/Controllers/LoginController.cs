using System.Text.Json;
using ExamenUno.Models;
using ExamenUno.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamenUno.Controllers
{
	public class LoginController : Controller
	{
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
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
					return View();
				}


			}catch (Exception ex){

				LoggerService.LogError(ex);
				ModelState.AddModelError(string.Empty, "Error al iniciar sesión intentalo más tarde.");
				return View();

			}
		}


		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login", "Login");
		}
	}
}
