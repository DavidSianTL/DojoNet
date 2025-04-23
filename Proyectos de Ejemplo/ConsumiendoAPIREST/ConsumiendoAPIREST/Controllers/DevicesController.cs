using Microsoft.AspNetCore.Mvc;
using ConsumiendoAPIREST.Models;
using ConsumiendoAPIREST.Services;
using System.Threading.Tasks;


namespace ConsumiendoAPIREST.Controllers
{
    public class DevicesController : Controller
    {
        private readonly DeviceService _service = new DeviceService();

        public async Task<ActionResult> Index()
        {
            var devices = await _service.GetAllDevicesAsync();
            return View(devices);
        }

        public async Task<ActionResult> Details(string id)
        {
            var device = await _service.GetDeviceByIdAsync(id);
            return View(device);
        }

        public ActionResult Create() => View();

        [HttpPost]
        public async Task<ActionResult> Create(Device device)
        {
            await _service.CreateDeviceAsync(device);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var device = await _service.GetDeviceByIdAsync(id);
            return View(device);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, Device device)
        {
            await _service.UpdateDeviceAsync(id, device);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(string id)
        {
            var device = await _service.GetDeviceByIdAsync(id);
            return View(device);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            await _service.DeleteDeviceAsync(id);
            return RedirectToAction("Index");
        }
    }
}
