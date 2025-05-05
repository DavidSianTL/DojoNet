using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

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
