using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Practica_JavaScript.Models
{
    public class UsuarioViewModel
    {
        private readonly string _ArchivoUsuario = Path.Combine(Directory.GetCurrentDirectory(), "sessionLogin.json");

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string UsrNombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string NombreCompleto { get; set; }

        public UsuarioViewModel ValidateUser(string usrnombre, string password)
        {
            var users = JsonConvert.DeserializeObject<List<UsuarioViewModel>>(File.ReadAllText(_ArchivoUsuario));
            return users?.FirstOrDefault(u => u.UsrNombre == usrnombre && u.Password == password);
        }

    }

}
