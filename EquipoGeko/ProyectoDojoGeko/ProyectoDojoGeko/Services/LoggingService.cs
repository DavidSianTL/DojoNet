using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly daoLogWSAsync _daoLog;

        public LoggingService(daoLogWSAsync daoLog)
        {
            _daoLog = daoLog;
        }

        public async Task RegistrarLogAsync(LogViewModel log)
        {
            await _daoLog.InsertarLogAsync(log);
        }

        public async Task<List<LogViewModel>> ObtenerLogsAsync()
        {
            return await _daoLog.ObtenerLogs();
        }
    }
}
