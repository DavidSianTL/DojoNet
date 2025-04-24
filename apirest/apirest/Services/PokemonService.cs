
using PokeApiConsumer.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokeApiConsumer.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://pokeapi.co/api/v2/pokemon";

        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PokemonListItem>> GetAllPokemonsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<PokemonListResponse>(_baseUrl);
            return response?.Results ?? new List<PokemonListItem>();
        }

        public async Task<Pokemon> GetPokemonByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Pokemon>($"{_baseUrl}/{id}");
        }

        public async Task<Pokemon> GetPokemonByNameAsync(string name)
        {
            return await _httpClient.GetFromJsonAsync<Pokemon>($"{_baseUrl}/{name}");
        }
    }
}