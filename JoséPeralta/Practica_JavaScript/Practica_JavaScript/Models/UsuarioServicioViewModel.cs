using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Practica_JavaScript.Models
{
    public class UsuarioServicioViewModel
    {
        private readonly string _ArchivoUsuario = Path.Combine(Directory.GetCurrentDirectory(), "sessionLogin.json");


        public UsuarioViewModel ValidateUser(string usrnombre, string password)
        {
            var users = JsonConvert.DeserializeObject<List<UsuarioViewModel>>(File.ReadAllText(_ArchivoUsuario));
            return users?.FirstOrDefault(u => u.UsrNombre == usrnombre && u.Password == password);
        }

    }
}
