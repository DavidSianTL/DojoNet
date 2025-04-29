using System.Text.Json.Serialization;

namespace UniversidadApiRest.Models
{
    public class University
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("state-province")]
        public string StateProvince { get; set; }

        [JsonPropertyName("alpha_two_code")]
        public string AlphaTwoCode { get; set; }

        [JsonPropertyName("domains")]
        public List<string> Domains { get; set; }

        [JsonPropertyName("web_pages")]
        public List<string> WebPages { get; set; }
    }
}
