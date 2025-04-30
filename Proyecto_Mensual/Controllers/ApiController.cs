using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppFuturista.Services;
using AppFuturista.Filters;

namespace AppFuturista.Controllers
{
    [ValidarSesion]
    public class ApiController : Controller
    {
        private readonly ApiRestService _apiRestService;
        private readonly ApiSoapService _apiSoapService;
        private readonly BitacoraService _bitacoraService;
        
        public ApiController(
            ApiRestService apiRestService, 
            ApiSoapService apiSoapService, 
            BitacoraService bitacoraService)
        {
            _apiRestService = apiRestService;
            _apiSoapService = apiSoapService;
            _bitacoraService = bitacoraService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Rest()
        {
            try
            {
                var productos = await _apiRestService.ObtenerProductosAsync();
                return View(productos);
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync("API", "Error al consultar API REST", ex);
                return RedirectToAction("Error", "Home", new { mensaje = "Error al consultar la API REST" });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Soap()
        {
            try
            {
                // Predefinimos algunos códigos de países para mostrar
                var paises = new[]
                {
                    new { Codigo = "ES", Nombre = "", Capital = "" },
                    new { Codigo = "US", Nombre = "", Capital = "" },
                    new { Codigo = "MX", Nombre = "", Capital = "" },
                    new { Codigo = "BR", Nombre = "", Capital = "" },
                    new { Codigo = "JP", Nombre = "", Capital = "" }
                };
                
                // Obtenemos la información de cada país
                for (int i = 0; i < paises.Length; i++)
                {
                    var codigo = paises[i].Codigo;
                    var nombreTarea = _apiSoapService.ObtenerNombrePaisPorCodigoAsync(codigo);
                    var capitalTarea = _apiSoapService.ObtenerCapitalPorCodigoAsync(codigo);
                    
                    await Task.WhenAll(nombreTarea, capitalTarea);
                    
                    paises[i] = new { Codigo = codigo, Nombre = nombreTarea.Result, Capital = capitalTarea.Result };
                }
                
                return View(paises);
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync("API", "Error al consultar API SOAP", ex);
                return RedirectToAction("Error", "Home", new { mensaje = "Error al consultar la API SOAP" });
            }
        }
    }
}
