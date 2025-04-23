using EjemploConsumoServicioSOAP.Service;
using Microsoft.AspNetCore.Mvc;

public class CapitalController : Controller
{
    private readonly ICountryInfo _countryInfo;
    public CapitalController(ICountryInfo countryInfo)
    {
        _countryInfo = countryInfo;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> ConsultarCapital(string codigoPais)
    {
        string resultado = string.Empty;
        try
        {
            resultado = await _countryInfo.ConsultarCapitalPorCodigo(codigoPais);
        }
        catch (Exception ex)
        {
            resultado = $"Error al consultar: {ex.Message}";
        }
        ViewBag.Resultado = resultado;
        return View("Index");
    }

    public async Task<IActionResult> Idiomas()
    {
        try
        {
            var idiomas = await _countryInfo.ListarIdiomasPorNombre();
            return View(idiomas); // Vista dedicada para idiomas
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View("Error"); // Vista de error genérica
        }
    }
    public async Task<IActionResult> IdiomasPorCodigo()
    {
        try
        {
            var idiomas = await _countryInfo.ListarIdiomasPorCodigo();
            return View("IdiomasPorCodigo", idiomas); // Vista específica
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View("Error");
        }
    }
    public async Task<IActionResult> NombreIdioma(string codigoIdioma)
    {
        try
        {
            if (string.IsNullOrEmpty(codigoIdioma))
                return View("ConsultarNombreIdioma"); // Vista para ingresar código

            var nombreIdioma = await _countryInfo.ObtenerNombreIdiomaPorCodigo(codigoIdioma);
            ViewBag.NombreIdioma = nombreIdioma;
            ViewBag.CodigoIdioma = codigoIdioma.ToUpper();
            return View("MostrarNombreIdioma"); // Vista para mostrar resultado
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View("Error");
        }
    }
    public async Task<IActionResult> CodigoISOLenguaje(string nombreIdioma)
    {
        try
        {
            if (string.IsNullOrEmpty(nombreIdioma))
                return View(); // Muestra formulario

            var codigoISO = await _countryInfo.ObtenerCodigoISOPorNombreIdioma(nombreIdioma);
            ViewBag.CodigoISO = codigoISO;
            ViewBag.NombreIdioma = nombreIdioma;
            return View("MostrarCodigoISO"); // Vista de resultados
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View("Error");
        }
    }
}
