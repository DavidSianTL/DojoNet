using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ConsumoApis.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Acción para buscar equipos desde TheSportsDB
        public async Task<IActionResult> BuscarEquipo(string nombre = "Arsenal")
        {
            var url = $"https://www.thesportsdb.com/api/v1/json/3/searchteams.php?t={nombre}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JObject.Parse(json);
                    ViewBag.Teams = data["teams"];
                }
                else
                {
                    ViewBag.Teams = null;
                    ViewBag.Error = $"Error al consultar API: {response.StatusCode}";
                }
            }

            return View();
        }
    }
}