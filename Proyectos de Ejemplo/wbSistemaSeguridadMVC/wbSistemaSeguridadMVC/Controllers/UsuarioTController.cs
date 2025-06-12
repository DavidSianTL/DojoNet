using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using wbSistemaSeguridadMVC.Data;
using wbSistemaSeguridadMVC.Helper;
using wbSistemaSeguridadMVC.Models;
using wbSistemaSeguridadMVC.Services;


namespace wbSistemaSeguridadMVC.Controllers
{
    public class UsuarioTController : Controller
    {
       

        private string connectionString = "Server=HOME_PF\\SQLEXPRESS;Database=SistemaSeguridad;Integrated Security=True;TrustServerCertificate=True;";
        private readonly daoToken _db;
        private readonly daoUsuarioT _dbUT;

        public UsuarioTController()
        {
            _db = new daoToken(connectionString);
            _dbUT = new daoUsuarioT(connectionString);
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(string usuario, string clave)
        {
            var usuarioValido = _dbUT.ValidarUsuario(usuario, clave); // Lógica simulada o desde DB
            if (usuarioValido != null)
            {
                int idSistema = 1;
                var jwtHelper = new JwtHelper();
                var tokenModel = jwtHelper.GenerarToken(usuarioValido.UsuarioLogin, usuarioValido.IdUsuario, 1);

               _db.GuardarToken(tokenModel);

                HttpContext.Session.SetString("Token", tokenModel.TokenS);
                HttpContext.Session.SetString("Usuario", usuarioValido.UsuarioLogin);

                return RedirectToAction("Index", "Sistema");
            }

            ViewBag.Mensaje = "Credenciales inválidas.";
            return View();
        }

        private UsuarioT ValidaUsuarioS(string usuario, string clave)
        {
            if (usuario == "admin" && clave == "1234")
            {
                return new UsuarioT
                {
                    IdUsuario = 1,
                    UsuarioLogin = "admin"
                };
            }

            return null;
        }
    }
}
