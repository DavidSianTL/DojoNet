using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace consumirApiRESTFUL.Models
{
    public class Device
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        [Required(ErrorMessage = "El campo nombre es requerido")]
        [StringLength(100, ErrorMessage ="Maximo 100 caracteres permitidos")]
        public string Name { get; set; }
        [JsonPropertyName("data")]
        public Dictionary<string, JsonElement> Data { get; set; }

    }
}
