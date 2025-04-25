using System.Net.Http.Headers;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIRESTPablo.Models;

namespace APIRESTPablo.Services
{
    public class MarvelService
    {
        private readonly HttpClient _httpClient;
        private const string API_URL = "https://gateway.marvel.com/v1/public/characters";
        private const string API_PUBLIC_KEY = "6840bd72d935f9fa3eb85327f30cf11b"; 
        private const string API_PRIVATE_KEY = "d59ad3812823df2716ec7b6e24dea6d897f1e38b"; 

        public MarvelService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<MarvelCharacter>> GetMarvelCharactersAsync()
        {
            
            var ts = DateTime.UtcNow.Ticks.ToString();
            var hash = GetMD5Hash(ts + API_PRIVATE_KEY + API_PUBLIC_KEY);

           
            var requestUrl = $"{API_URL}?ts={ts}&apikey={API_PUBLIC_KEY}&hash={hash}";

           
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode(); 

            var content = await response.Content.ReadAsStringAsync();

           
            using var doc = JsonDocument.Parse(content);

            
            var characters = doc.RootElement
                .GetProperty("data")
                .GetProperty("results")
                .EnumerateArray()
                .Select(c => new MarvelCharacter
                {
                    Name = c.GetProperty("name").GetString(),
                    Description = c.GetProperty("description").GetString(),
                    Thumbnail = c.GetProperty("thumbnail").GetProperty("path").GetString() + "." + c.GetProperty("thumbnail").GetProperty("extension").GetString()
                })
                .ToList();

            return characters;
        }

        
        private string GetMD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder();
                foreach (var b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}





