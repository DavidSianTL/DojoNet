using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace _Evaluacion_Mensual_Abril.Models
{
    public class UserViewModel
    {
        private readonly string _ArchivoUsuario = Path.Combine(Directory.GetCurrentDirectory(), "users.json");

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string UsrNombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string NombreCompleto { get; set; }

        public UserViewModel ValidateUser(string usrnombre, string password)
        {
            var users = JsonConvert.DeserializeObject<List<UserViewModel>>(File.ReadAllText(_ArchivoUsuario));

            var user = users?.FirstOrDefault(u => u.UsrNombre == usrnombre);

            if (user == null)
            {
                throw new Exception("El usuario no existe.");
            }

            if (user.Password != password)
            {
                throw new Exception("La contraseña es incorrecta.");
            }

            return user;
        }


    }

    public class UserJSON
    {
        // Se define la ruta al archivo 'users.json' donde se almacenan los usuarios.
        // Utiliza el directorio actual y lo combina con el nombre del archivo.
        private readonly string _rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "users.json");
        // Método para obtener la lista de usuarios desde el archivo JSON
        public List<UserViewModel> ObtenerUsuarios()
        {
            if (File.Exists(_rutaArchivo))
            {
                var json = System.IO.File.ReadAllText(_rutaArchivo);
                var usuarios = JsonConvert.DeserializeObject<List<UserViewModel>>(json);
                return usuarios ?? new List<UserViewModel>();
            }
            else
            {
                return new List<UserViewModel>();
            }
        }
    }

    public class UsersService
    {
        // Se define la ruta al archivo 'users.json' donde se almacenan los usuarios.
        private readonly string _rutaArchivo;

        public UsersService()
        {
            _rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "users.json");
        }

        public List<UserViewModel> ObtenerUsuarios()
        {
            if (!File.Exists(_rutaArchivo))
            {
                return new List<UserViewModel>();
            }

            var jsonData = File.ReadAllText(_rutaArchivo);
            return System.Text.Json.JsonSerializer.Deserialize<List<UserViewModel>>(jsonData) ?? new List<UserViewModel>();
        }

        
        public UserViewModel ObtenerUsuarioPorNombre(string usrNombre)
        {
            var usuarios = ObtenerUsuarios(); // lista de usuarios
            return usuarios.FirstOrDefault(u => u.UsrNombre == usrNombre);
        }


        public void GuardarUsuario(UserViewModel usuario)
        {
            var usuarios = ObtenerUsuarios();
            usuarios.Add(usuario);
            var jsonData = System.Text.Json.JsonSerializer.Serialize(usuarios);
            File.WriteAllText(_rutaArchivo, jsonData);
        }


    }


}