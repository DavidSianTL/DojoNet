using Microsoft.AspNetCore.Mvc.Filters;
using ProyectoDojoGeko.Data;

// Heredamos de IAsyncActionFilter para crear un filtro de acción asíncrono
// que nos permite ejecutar código antes y después de la acción del controlador
public class FotoSession : IAsyncActionFilter
{
    // Inyectamos el DAO de Empleado para obtener la foto de perfil
    private readonly daoEmpleadoWSAsync _daoEmpleado;

    // Inyectamos el DAO de Empresa para obtener el logo de la empresa 
    private readonly daoEmpresaWSAsync _daoEmpresa;

    // Constructor que recibe el DAO de Empleado y el DAO de Empresa
    public FotoSession(daoEmpleadoWSAsync daoEmpleado, daoEmpresaWSAsync daoEmpresa)
    {
        _daoEmpleado = daoEmpleado;
        _daoEmpresa = daoEmpresa;
    }

    // Usamos "ActionExecutionAsync" para ejecutar código antes y después de la acción
    // Usamos "ActionExecutionDelegate" para continuar con la ejecución de la acción
    // De esta manera, podemos agregar la foto de perfil al contexto de la acción
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Verificamos si la sesión contiene el IdEmpleado y el IdEmpresa
        var session = context.HttpContext.Session;
        var idEmpleado = session.GetInt32("IdEmpleado") ?? 0;
        if (idEmpleado != 0)
        {
            // Extraemos los datos del empleado para obtener la foto de perfil
            var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(idEmpleado);
            // Aquí traes la URL de Cloudinary
            var foto = string.IsNullOrEmpty(empleado?.Foto)
                ? "/imagenes/perfiles/default.png" // Imagen por defecto local y/o de Cloudinary
                : empleado.Foto; // Esta es la URL de Cloudinary
            context.HttpContext.Items["FotoPerfilUrl"] = foto; // Guardamos la URL de la foto de perfil en el contexto
        }

        // Logo de la empresa
        var idEmpresa = session.GetInt32("IdEmpresa") ?? 0;
        if (idEmpresa != 0)
        {
            var empresa = await _daoEmpresa.ObtenerEmpresaPorIdAsync(idEmpresa);
            var logo = string.IsNullOrEmpty(empresa?.Logo)
                ? "/imagenes/logos/default.png"
                : empresa.Logo; // URL Cloudinary o local
            context.HttpContext.Items["LogoEmpresaUrl"] = logo;
        }

        // Continuamos con la ejecución de la acción
        await next();
    }
}