using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using ExamDaniel.Models;
using System.Net.Http;

public class ProductoApiRestServicio
{
    private readonly HttpClient _httpClient;
    private const string baseUrl = "https://fakestoreapi.com/products";

    public ProductoApiRestServicio(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProductoApiRest>> ObtenerTodosAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();  // Lanza una excepción si el código de estado no es exitoso
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ProductoApiRest>>(json);
        }
        catch (Exception ex)
        {
            // Log error aquí o devolver un valor por defecto
            Console.WriteLine($"Error al obtener productos: {ex.Message}");
            return new List<ProductoApiRest>(); // Retornar una lista vacía en caso de error
        }
    }

    public async Task<ProductoApiRest> ObtenerPorIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{baseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProductoApiRest>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener producto por ID: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CrearAsync(ProductoApiRest producto)
    {
        try
        {
            var json = JsonSerializer.Serialize(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(baseUrl, content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear producto: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> EditarAsync(int id, ProductoApiRest producto)
    {
        try
        {
            var json = JsonSerializer.Serialize(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{baseUrl}/{id}", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al editar producto ID {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar producto ID {id}: {ex.Message}");
            return false;
        }
    }
}
