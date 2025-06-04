using System.Net;

namespace SistemaAutenticacion.Middleware
{
    public class MiddleException: Exception
    {
        public HttpStatusCode Codigo { get; set; }
        public object? Errores { get; set; }

        public MiddleException(HttpStatusCode codigo, object? errores)
        {
            Codigo = codigo;
            Errores = errores;
        }
    }
}
