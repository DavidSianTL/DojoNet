using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamenUno.Services
{

	public interface ISessionService
	{
		public IActionResult? validateSession(HttpContext context);
		

    }

    public  class SessionService : ISessionService
    {
		public  IActionResult? validateSession(HttpContext context)
		{
			var userSession = context.Session.GetString("User");
			if (string.IsNullOrEmpty(userSession) || string.IsNullOrWhiteSpace(userSession))
			{
				return new RedirectToActionResult("Login", "Login", null);
			}

			return null;

		}
	}
}
