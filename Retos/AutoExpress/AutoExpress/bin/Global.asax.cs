using System;
using System.Web;

namespace AutoExpress.Servicio
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciar la aplicación
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando se inicia una nueva sesión
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Código que se ejecuta al inicio de cada solicitud
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando se autentica una solicitud
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando se produce un error no controlado
            Exception ex = Server.GetLastError();
            // Log del error aquí si es necesario
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando finaliza una sesión
        }

        protected void Application_End(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando finaliza la aplicación
        }
    }
}
