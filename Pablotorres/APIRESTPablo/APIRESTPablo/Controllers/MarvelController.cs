using APIRESTPablo.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIRESTPablo.Controllers
{
    public class MarvelController : Controller
    {
        private readonly MarvelService _marvelService;

        public MarvelController(MarvelService marvelService)
        {
            _marvelService = marvelService;
        }

        public async Task<IActionResult> Index()
        {
            var characters = await _marvelService.GetMarvelCharactersAsync();
            return View(characters);
        }
    }
}


