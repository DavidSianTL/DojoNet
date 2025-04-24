using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MiProyectoApi.Services
{
    // Modelo para los posts de JSONPlaceholder
    public class Post
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }

    public class JsonPlaceholderService
    {
        private readonly HttpClient _httpClient;

        public JsonPlaceholderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método para obtener la lista de posts
        public async Task<List<Post>> GetPostsAsync()
        {
            // Realiza una solicitud GET a la API pública
            var posts = await _httpClient.GetFromJsonAsync<List<Post>>("https://jsonplaceholder.typicode.com/posts");
            return posts;
        }
    }
}
