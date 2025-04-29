using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CRUD_Evaluacion_Mensual_Abril.Models
{
    public class UsuarioServicio
    {
        private readonly string _ArchivoUsuario = Path.Combine(Directory.GetCurrentDirectory(), "users.json");


        public UsuarioModel ValidateUser(string usrnombre, string password)
        {
            var users = JsonConvert.DeserializeObject<List<UsuarioModel>>(File.ReadAllText(_ArchivoUsuario));
            return users?.FirstOrDefault(u => u.UsrNombre == usrnombre && u.Password == password);
        }
    }
}
