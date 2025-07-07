using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Services.Interfaces
{
    public interface ILoggingService
    {
        Task RegistrarLogAsync(LogViewModel log);
        Task<List<LogViewModel>> ObtenerLogsAsync();
    }
}
