using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class SesionActivaMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string archivoSesion = "sesiones.json";

    public SesionActivaMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var usrNombre = context.Session.GetString("UsrNombre");
        var tokenSesion = context.Session.GetString("SesionToken");

        if (!string.IsNullOrEmpty(usrNombre) && !string.IsNullOrEmpty(tokenSesion))
        {
            Dictionary<string, string> sesiones = LeerSesiones();

            if (sesiones.TryGetValue(usrNombre, out var tokenGuardado))
            {
                if (tokenGuardado != tokenSesion)
                {
                    // Otra sesión está activa
                    context.Session.Clear();
                    context.Response.Redirect("/Login/Login?expirada=true");
                    return;
                }
            }
        }

        await _next(context);
    }

    private Dictionary<string, string> LeerSesiones()
    {
        if (!File.Exists(archivoSesion)) return new Dictionary<string, string>();

        string json = File.ReadAllText(archivoSesion);
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
    }
}
