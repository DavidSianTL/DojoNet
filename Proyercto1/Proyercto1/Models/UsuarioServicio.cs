using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Proyercto1.Models
{
    public class UsuarioServicio
    {
        private readonly string _ArchivoUsuarios = Path.Combine(Directory.GetCurrentDirectory(), "users.json");


        public Usuario ValidateUser(string usrnombre, string password)
        {
            var users = JsonConvert.DeserializeObject<List<Usuario>>(File.ReadAllText(_ArchivoUsuarios));
            return users?.FirstOrDefault(u => u.UsrNombre == usrnombre && u.Password == password);
        }


    }
}
