using Microsoft.AspNetCore.Mvc;
using ServiceReference1;
using _Evaluacion_Mensual_Abril.Models;
using System.Linq;

namespace _Evaluacion_Mensual_Abril.Controllers
{
    public class CountryController : Controller
    {
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var client = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);
            var result = client.ListOfCountryNamesByNameAsync().Result;
            var countries = result.Body.ListOfCountryNamesByNameResult;

            var paged = new PagedResult<tCountryCodeAndName>
            {
                Items = countries.Skip((page - 1) * pageSize).Take(pageSize),
                PageNumber = page,
                TotalPages = (int)Math.Ceiling(countries.Length / (double)pageSize)
            };

            return View(paged);
        }
    }
}




