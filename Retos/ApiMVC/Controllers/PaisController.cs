using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc; 
using Newtonsoft.Json;
using ProyectoPaises.Models;

namespace ProyectoPaises.Controllers
{
    public class PaisController : Controller

    {
        private readonly string apiUrl = "https://restcountries.com/v3.1/all";

        public async Task<ActionResult> Index()
        {
            List<Pais> paises = new List<Pais>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    paises = JsonConvert.DeserializeObject<List<Pais>>(json);
                }
            }

            return View(paises);
        }
    }
}
