using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PracticaApiRest.Models
{
    public class UniversitiesModel
    {

        [JsonPropertyName("name")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Name { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("web_page")]
        public List<string> WebPages { get; set; }

        [JsonPropertyName("domains")]
        public List<string> Domains { get; set; }

        [JsonPropertyName("state-province")]
        public string StateProvince { get; set; }

        [JsonPropertyName("alpha_two_code")]
        public string AlphaTwoCode { get; set; }

    }
}
