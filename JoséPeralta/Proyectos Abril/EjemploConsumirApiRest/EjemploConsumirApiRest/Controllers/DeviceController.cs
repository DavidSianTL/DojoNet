using EjemploConsumirApiRest.Services;
using Microsoft.AspNetCore.Mvc;

namespace EjemploConsumirApiRest.Controllers
{
    public class DeviceController : Controller
    {

        private readonly DeviceService _deviceService = new DeviceService();

        public async Task<IActionResult> Index()
        {
            var devices = await _deviceService.GetAllDevicesAsync();

            return View("~/Views/Device/Index.cshtml", devices);

        }

    }
}
