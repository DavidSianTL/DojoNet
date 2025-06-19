using System.Net;

namespace SistemaAutenticacion.Middleware
{
    public class MiddleException : Exception
    {
        
        // Definimos una propiedad para el mensaje de error
        public HttpStatusCode StatusCode { get; set; }

        // Definimos una propiedad de tipo object para almacenar los errores
        public Object? Errores { get; set; }

        // Constructor de la clase MiddleException que recibe un mensaje de error y un código de estado
        public MiddleException(HttpStatusCode statusCode, Object? errores = null)
        {
            // Asignamos el mensaje de error a la propiedad Errores
            Errores = errores;
            // Asignamos el código de estado a la propiedad StatusCode
            StatusCode = statusCode;
        }


    }
}