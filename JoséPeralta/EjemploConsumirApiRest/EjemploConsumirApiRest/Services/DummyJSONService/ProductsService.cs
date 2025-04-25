using System;
using EjemploConsumirApiRest.Models.DummyJSONModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class ProductsService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://dummyjson.com"; // URL de la API DummyJSON

    // El hhtpClient se inyecta en el constructor
    // Esto permite que el HttpClient sea reutilizable y se gestione adecuadamente
    // Y proviene de la inyección de dependencias en el Program.cs
    public ProductsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProductsViewModel>> GetProductsAsync()
    {
        // Reemplazar las comillas invertidas (`) con comillas dobles (") para corregir el error CS1056
        var response = await _httpClient.GetAsync($"{_baseUrl}/products");
        //var response = await _httpClient.GetFromJsonAsync<List<ProductsViewModel>>($"{_baseUrl}/products");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductResponse>(json);
            return result.Products;
        }

        return new List<ProductsViewModel>();
    }

    // Creamos el método para crear un producto
    // Pasamos el modelo de producto como parámetro
    // Y lo convertimos a JSON para enviarlo en el cuerpo de la solicitud
    // El método devuelve el producto creado o null si hubo un error
    public async Task<ProductsViewModel> AddProductAsync(ProductsViewModel product)
    {

        // Convertimos el producto a JSON
        var json = JsonConvert.SerializeObject(product);

        // Creamos el contenido de la solicitud
        var responseContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        // Enviamos la solicitud POST a la API
        var response = await _httpClient.PostAsync($"{_baseUrl}/products/add", responseContent);

        // Verificamos si la respuesta es exitosa
        if (response.IsSuccessStatusCode)
        {
            // Leemos el contenido de la respuesta
            var responseJson = await response.Content.ReadAsStringAsync();
            // Deserializamos el JSON a un objeto ProductsViewModel
            var createdProduct = JsonConvert.DeserializeObject<ProductsViewModel>(responseJson);
            return createdProduct;
        }

        // Si hubo un error, devolvemos null
        return null;

    }

    // Creamos el método para editar un producto
    // Pasamos el id del producto y el modelo de producto como parámetros
    public async Task<ProductsViewModel> UpdatedProductAsync(int idProduct, ProductsViewModel product)
    {

        // Convertimos el producto a json
        var json = JsonConvert.SerializeObject(product);

        // Creamos el contenido de la solicitud
        var responseContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        // Enviamos la solicitud POST a la API
        var response = await _httpClient.PutAsync($"{_baseUrl}/products/{idProduct}", responseContent);

        // Verificamos si la respuesta es exitosa
        if (response.IsSuccessStatusCode)
        {
            // Leemos el contenido de la respuesta
            var responseJson = await response.Content.ReadAsStringAsync();

            // Deserializamos el JSON a un objeto ProductsViewModel
            var updatedProduct = JsonConvert.DeserializeObject<ProductsViewModel>(responseJson);
            return updatedProduct;

        }

        // Si hubo un error, devolvemos null
        return null;


    }





}
