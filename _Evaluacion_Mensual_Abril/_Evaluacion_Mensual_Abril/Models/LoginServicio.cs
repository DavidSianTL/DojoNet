using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace _Evaluacion_Mensual_Abril.Models
{
    public class LoginServicio
    {
        private readonly string _archivoUsuario = Path.Combine(Directory.GetCurrentDirectory(), "usuarios.json");

        public Login ValidateUser(string email, string password)
        {
            var json = File.ReadAllText(_archivoUsuario);
            var usuarios = JsonConvert.DeserializeObject<List<Login>>(json);
            return usuarios?.FirstOrDefault(u => u.Username == email && u.Password == password);
        }
    }
}
