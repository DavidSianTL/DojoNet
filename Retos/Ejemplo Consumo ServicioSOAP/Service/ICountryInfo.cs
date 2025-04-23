using System.Threading.Tasks;

namespace EjemploConsumoServicioSOAP.Service
{
    public interface ICountryInfo
    {
        Task<string> ConsultarCapitalPorCodigo(string codigoPais);
        Task<string> ObtenerNombrePais(string codigoPais);
        Task<string> ObtenerMonedaPais(string codigoPais);
        Task<string> ObtenerBanderaPais(string codigoPais);
        Task<string> ListarContinentesPorNombre();
        Task<string> ListarContinentesPorCodigo();
        Task<string> ListarMonedasPorNombre();
        Task<string> ListarMonedasPorCodigo();
        Task<string> ListarPaisesPorCodigo();
        Task<string> ListarPaisesPorNombre();
        Task<string> ListarPaisesAgrupadosPorContinente();
        Task<string> ObtenerNombreIdioma(string codigoIdioma);
        Task<string> ObtenerIdiomasPais(string codigoPais);
    }
}