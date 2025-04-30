using System.IO;
using System.Linq;
using Final_Evaluacion_Mensual_Abril.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Final_Evaluacion_Mensual_Abril.Models
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
