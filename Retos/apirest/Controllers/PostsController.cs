using Microsoft.AspNetCore.Mvc;
using MiProyectoApi.Services;
using System.Threading.Tasks;

namespace MiProyectoApi.Controllers
{
    public class PostsController : Controller
    {
        private readonly JsonPlaceholderService _jsonService;

        public PostsController(JsonPlaceholderService jsonService)
        {
            _jsonService = jsonService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _jsonService.GetPostsAsync();
            return View(posts);
        }
    }
}
