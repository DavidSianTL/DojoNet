using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using wsAuthentication.Models;

namespace wsAuthentication
{
    /// <summary>
    /// Descripción breve de wsUsuarios
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class wsUsuarios : System.Web.Services.WebService
    {

        // SIMULAR datos de prueba
        private static readonly List<Users> users = new List<Users>()
        {
            new Users
            {
                Id = 1,
                username = "Junior",
                email = "junior@gmail.com",
                password = "2332"
            }
        };


        [WebMethod(Description = "Valida las credenciales de un usuario")]
        public LoginResponse ValidateLogin(string username, string password)
        {
            var user = users.FirstOrDefault(usuario => usuario.username == username && usuario.password == password);

            if (user != null)
            {
                return new LoginResponse
                {
                    Success = true,
                    Messages = $"Bienvenido {user.username}",
                    Nombre = user.username,
                    Email = user.email
                };
            }
            else
            {
                return new LoginResponse
                {
                    Success = false,
                    Messages = $"No se encontraron datos",
                    Nombre = null,
                    Email = null
                };
            }

        }
    }
}
