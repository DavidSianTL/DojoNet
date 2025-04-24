using Microsoft.AspNetCore.Mvc;
using PokeApiConsumer.Models;
using PokeApiConsumer.Services;
using System.Threading.Tasks;

namespace PokeApiConsumer.Controllers
{
    public class PokemonsController : Controller
    {
        private readonly PokemonService _service;

        public PokemonsController(PokemonService service)
        {
            _service = service;
        }

        public async Task<ActionResult> Index()
        {
            var pokemons = await _service.GetAllPokemonsAsync();
            return View(pokemons);
        }

        public async Task<ActionResult> Details(int id)
        {
            var pokemon = await _service.GetPokemonByIdAsync(id);
            if (pokemon == null)
            {
                return NotFound();
            }
            return View(pokemon);
        }

        public async Task<ActionResult> DetailsByName(string name)
        {
            var pokemon = await _service.GetPokemonByNameAsync(name);
            if (pokemon == null)
            {
                return NotFound();
            }
            return View("Details", pokemon);
        }
    }
}