using _Evaluacion_Mensual_Abril.Models.FakeStore;
using Newtonsoft.Json;
namespace _Evaluacion_Mensual_Abril.Services.FakeStore
{
    public class FSProductService
    {

        // Definimos la variable HttpClient para realizar las peticiones HTTP
        // Y la URL base de la API
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://fakestoreapi.com"; // URL de la API FakeStoreApi

        /// Constructor que recibe el HttpClient inyectado
        public FSProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Creamos la función que obtiene todos los productos
        public async Task<List<FSProductsViewModel>> GetAllProductsAsync()
        {
            // Realizamos la petición GET a la API para obtener todos los productos
            var response = await _httpClient.GetAsync($"{_baseUrl}/products");

            // Verificamos si la respuesta fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Leemos el contenido de la respuesta como una cadena
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // Deserializamos la cadena JSON a una lista de productos
                var products = JsonConvert.DeserializeObject<List<FSProductsViewModel>>(jsonResponse);
                // Retornamos la lista de productos
                return products;
            }

            // Si la respuesta no fue exitosa, retornamos una lista vacía
            return new List<FSProductsViewModel>();


        }

        // Creamos la función para 'Crear' un producto
        public async Task<FSProductsViewModel> AddProductAsync(FSProductsViewModel product)
        {
            // Realizamos la petición POST a la API para crear un nuevo producto
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/products", product);
            // Verificamos si la respuesta fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Leemos el contenido de la respuesta como una cadena
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // Deserializamos la cadena JSON a un producto
                var createdProduct = JsonConvert.DeserializeObject<FSProductsViewModel>(jsonResponse);
                // Retornamos el producto creado
                return createdProduct;
            }
            // Si la respuesta no fue exitosa, retornamos null
            return null;
        }

        // Creamos la función para 'Editar' un producto
        public async Task<FSProductsViewModel?> UpdateProductAsync(int id, FSProductsViewModel product)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/products/{id}", product);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FSProductsViewModel>(jsonResponse);
            }
            return null;
        }

        // Creamos la función para 'Eliminar' un producto
        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/products/{id}");
            return response.IsSuccessStatusCode;
        }




    }
}
