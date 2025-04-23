using CountryInfoService;
using EjemploApiSoap.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EjemploConsumoServicioSOAP.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ICountryInfo _countryInfo;
        public LanguageController(ICountryInfo countryInfo)
        {
            _countryInfo = countryInfo;
        }

        public IActionResult Index()
        {
            return View("~/Views/Languages/Index.cshtml");
        }

        public async Task<IActionResult> ConsultarIdiomasPorNombre()
        {
            List<tLanguage> languages = new List<tLanguage>();
            try
            {
                languages = await _countryInfo.ListaDeIdiomasPorNombre();
                if (languages == null || !languages.Any())
                {
                    ViewBag.Error = "No se encontraron idiomas.";
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = $"Error al consultar: {e.Message}";
                System.IO.File.AppendAllText("log.txt", e.ToString());
            }
            ViewBag.Languages = languages;
            ViewBag.LanguagesJson = JsonConvert.SerializeObject(languages);
            return View("~/Views/Languages/LanguagesByName.cshtml");

        }

        public async Task<IActionResult> ConsultarIdiomasPorCodigo()
        {
            List<tLanguage> languages = new List<tLanguage>();

            try
            {
                languages = await _countryInfo.ListaDeIdiomasPorCodigo();

                if (languages == null || !languages.Any())
                {
                    ViewBag.Error = "No se encontraron idiomas.";
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = $"Error al consultar: {e.Message}";
                System.IO.File.AppendAllText("log.txt", e.ToString());
            }

            ViewBag.Languages = languages;
            ViewBag.LanguagesJson = JsonConvert.SerializeObject(languages);

            return View("~/Views/Languages/LanguagesByCode.cshtml");
        }


        public async Task<IActionResult> VistaConsultarIdioma()
        {
            return View("~/Views/Languages/LanguageName.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ConsultarIdioma(string idioma)
        {
            string resultado = string.Empty;
            try
            {

                if (string.IsNullOrEmpty(idioma)) {
                    ViewBag.Error = "El idioma no puede ser nulo.";
                    return View("~/Views/Languages/LanguageName.cshtml");
                } 

                resultado = await _countryInfo.ConsultarIdioma(idioma);
                if (string.IsNullOrEmpty(resultado))
                {
                    ViewBag.Error = "No se encontró el idioma.";
                }

            }
            catch (Exception e)
            {
                ViewBag.Error = $"Error al consultar: {e.Message}";
                System.IO.File.AppendAllText("log.txt", e.ToString());
            }
            ViewBag.Resultado = resultado;
            return View("~/Views/Languages/LanguageName.cshtml");
        }



        
    }
}
