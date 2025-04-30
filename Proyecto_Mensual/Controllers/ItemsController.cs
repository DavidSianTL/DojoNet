using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RazorCrudApp.Models;
using RazorCrudApp.Services;

namespace RazorCrudApp.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ItemService _itemService;
        
        public ItemsController(ItemService itemService)
        {
            _itemService = itemService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            
            await _itemService.CreateItemAsync(item);
            
            return RedirectToAction("Index", "Dashboard");
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            
            var item = await _itemService.GetItemByIdAsync(id);
            
            if (item == null)
            {
                return NotFound();
            }
            
            return View(item);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(string id, Item item)
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            
            var updatedItem = await _itemService.UpdateItemAsync(id, item);
            
            if (updatedItem == null)
            {
                return NotFound();
            }
            
            return RedirectToAction("Index", "Dashboard");
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            
            var result = await _itemService.DeleteItemAsync(id);
            
            if (!result)
            {
                return NotFound();
            }
            
            return RedirectToAction("Index", "Dashboard");
        }
        
        // AJAX endpoints for modal operations
        [HttpGet]
        public async Task<IActionResult> GetItem(string id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            
            if (item == null)
            {
                return NotFound();
            }
            
            return Json(item);
        }
    }
}
