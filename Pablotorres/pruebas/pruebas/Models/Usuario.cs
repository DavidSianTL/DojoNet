using Newtonsoft.Json;
namespace pruebas.Models
{

    public class Usuario
    {

        public required string Username { get; set; }
        
        public required string Password { get; set; }

        public string NombreCompleto { get; set; }


     
    }

}
