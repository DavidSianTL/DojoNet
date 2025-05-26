namespace DatabaseConnection.Services
{
	public interface IConnectionServiceAsync
	{
		string GetConnection();
	}
	public class ConnectionServiceAsync : IConnectionServiceAsync
	{
		public string GetConnection()
		{
			return "Server=SKINOFME;Database=EmpresaDB;Trusted_Connection=True;TrustServerCertificate=True";
		}

	}
}
