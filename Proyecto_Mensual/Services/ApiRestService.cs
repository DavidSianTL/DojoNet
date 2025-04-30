using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AppFuturista.Models;
using Microsoft.Extensions.Logging;

namespace AppFuturista.Services
{
    public class ApiRestService
    {
        private readonly HttpClient _httpClient;
        private readonly BitacoraService _bitacoraService;
        private readonly ILogger<ApiRestService> _logger;
        private readonly string _baseUrl;
        
        public ApiRestService(
            HttpClient httpClient, 
            BitacoraService bitacoraService, 
            ILogger<ApiRestService> logger,
            ArchivoJsonService archivoJsonService)
        {
            _httpClient = httpClient;
            _bitacoraService = bitacoraService;
            _logger = logger;
            
            var configuracion = archivoJsonService.LeerConfiguracionAsync<ConfiguracionApp>("configuracion.json").Result;
            _baseUrl = configuracion.UrlApiRest;
        }
        
        public async Task<List<ProductoApiDTO>> ObtenerProductosAsync()
        {
            try
            {
                await _bitacoraService.RegistrarEventoAsync("API REST", $"Consultando productos desde {_baseUrl}/products");
                
                var productos = await _httpClient.GetFromJsonAsync<List<ProductoApiDTO>>($"{_baseUrl}/products");
                
                await _bitacoraService.RegistrarEventoAsync("API REST", $"Obtenidos {productos.Count} productos de la API");
                
                return productos;
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync("API REST", "Error al obtener productos", ex);
                _logger.LogError(ex, "Error al obtener productos de la API REST");
                throw;
            }
        }
        
        public async Task<ProductoApiDTO> ObtenerProductoPorIdAsync(int id)
        {
            try
            {
                await _bitacoraService.RegistrarEventoAsync("API REST", $"Consultando producto con ID {id} desde {_baseUrl}/products/{id}");
                
                var producto = await _httpClient.GetFromJsonAsync<ProductoApiDTO>($"{_baseUrl}/products/{id}");
                
                await _bitacoraService.RegistrarEventoAsync("API REST", $"Obtenido producto: {producto.Title}");
                
                return producto;
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync("API REST", $"Error al obtener producto con ID {id}", ex);
                _logger.LogError(ex, "Error al obtener producto de la API REST");
                throw;
            }
        }
        
        public async Task<List<string>> ObtenerCategoriasAsync()
        {
            try
            {
                await _bitacoraService.RegistrarEventoAsync("API REST", $"Consultando categorías desde {_baseUrl}/products/categories");
                
                var categorias = await _httpClient.GetFromJsonAsync<List<string>>($"{_baseUrl}/products/categories");
                
                await _bitacoraService.RegistrarEventoAsync("API REST", $"Obtenidas {categorias.Count} categorías de la API");
                
                return categorias;
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync("API REST", "Error al obtener categorías", ex);
                _logger.LogError(ex, "Error al obtener categorías de la API REST");
                throw;
            }
        }
    }
}
