using Microsoft.AspNetCore.Mvc;

namespace consumirApiRESTFUL.Controllers
{
    public class DevicesControler : Controller
    {

        private readonly DeviceService _service = new DeviceService();

        public async Task<ActionResult> Index()
        {
            var devices = await _service.GetAllDeviceAsync();
            return View(devices);

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
