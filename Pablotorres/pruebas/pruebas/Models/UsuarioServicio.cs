using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace pruebas.Models
{
    public class UsuarioServicio
    {
        private readonly string _ArchivoUsuario = Path.Combine(Directory.GetCurrentDirectory(), "usuarios.json");
        public Usuario ValidateUser(string email, string password)
        {
            var users = JsonConvert.DeserializeObject<List<Usuario>>(File.ReadAllText(_ArchivoUsuario));
            return users?.FirstOrDefault(u => u.Password == password);
        }

    }
}
