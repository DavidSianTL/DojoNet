namespace MiBanco.Services
{
	public interface ILogService
	{
		void LogError(int codigoError, string mensaje);
		void LogWarning(int statusCode, string mensaje);
		void LogInfo(int statusCode, string mensaje);
	}

	public class LogService : ILogService

	{
		private string _logFilePath = Path.Combine(Directory.GetCurrentDirectory(),"Logs", "Log.txt");
		
		private void GuatdarLog(string log)
		{
			File.AppendAllText(_logFilePath, log);
        }

        public void LogError(int statusCode, string mensaje)
		{
			var log = $"ERROR   | Hora: {DateTime.Now} | codigo: {statusCode} | mensaje: {mensaje}\n";

			GuatdarLog(log);
        }

		public void LogWarning(int statusCode, string mensaje)
		{
			var log = $"WARNING | Hora: {DateTime.Now} | Código: {statusCode} | mensaje: {mensaje}\n";
			GuatdarLog(log);
        }

		public void LogInfo(int statusCode, string mensaje)
		{
			var log = $"INFO    | Hora: {DateTime.Now} | Código: {statusCode} | mensaje: {mensaje}\n";
			GuatdarLog(log);
        }
	}
}
