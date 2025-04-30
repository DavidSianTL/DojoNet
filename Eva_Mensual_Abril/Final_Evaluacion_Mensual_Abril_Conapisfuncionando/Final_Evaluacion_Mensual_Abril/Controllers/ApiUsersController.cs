using Final_Evaluacion_Mensual_Abril.Models;
using Final_Evaluacion_Mensual_Abril.Services;
using Microsoft.AspNetCore.Mvc;

namespace Final_Evaluacion_Mensual_Abril.Controllers
{
    public class ApiUsersController : Controller
    {
        private readonly FakeStoreService _fakeStoreService;

        public ApiUsersController(FakeStoreService fakeStoreService)
        {
            _fakeStoreService = fakeStoreService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _fakeStoreService.GetUsersAsync();
            return View(users);
        }

        
    }
}