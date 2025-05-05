using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EjemploConsumirApiRest.Models
{
    public class ServiceViewModel
    {
        [JsonPropertyName("id")]
        [Required]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        [Required(ErrorMessage = "El nombre del producto no puede faltar")]
        [StringLength(100, ErrorMessage = "El nombre del producto no puede tener más de 50 caracteres")]
        public string Name { get; set; }

        [JsonPropertyName("data")]
        public Dictionary<string, JsonElement> Data { get; set; }
    }
}
