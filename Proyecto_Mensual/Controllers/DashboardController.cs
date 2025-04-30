using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RazorCrudApp.Services;

namespace RazorCrudApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        
        public DashboardController(ItemService itemService, UserService userService)
        {
            _itemService = itemService;
            _userService = userService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            
            var items = await _itemService.GetAllItemsAsync();
            return View(items);
        }
    }
}
