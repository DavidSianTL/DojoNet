using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace TiendaEnLinea_MR.Models
{
    public class UsuarioServicioModel
    {
        private readonly string _ArchivoUsuario = Path.Combine(Directory.GetCurrentDirectory(), "users.json");


        public UsuarioViewModel ValidateUser(string correo, string password)
        {
            var users = JsonConvert.DeserializeObject<List<UsuarioViewModel>>(File.ReadAllText(_ArchivoUsuario));
            return users?.FirstOrDefault(u => u.Correo == correo && u.Password == password);
        }


    }
}
