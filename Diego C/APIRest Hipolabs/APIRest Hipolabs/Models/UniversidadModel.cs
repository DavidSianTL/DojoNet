using System.Text.Json.Serialization;

namespace APIRest_Hipolabs.Models
{
    public class UniversidadModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("web_pages")]
        public string[] WebPages { get; set; }

        [JsonPropertyName("alpha_two_code")]
        public string AlphaTwoCode { get; set; }
    }
}
