using Newtonsoft.Json;

namespace _Evaluacion_Mensual_Abril.Models
{
    public class Login
    {
        [JsonProperty("Email")]
        public string Username { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("NombreCompleto")]
        public string NombreCompleto { get; set; }
    }
}

