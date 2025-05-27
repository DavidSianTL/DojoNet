using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseConnection.Services
{
	public interface IConnectionServiceAsync
	{
        Task<DataSet> ExecuteSelectAsync(string query);
        string GetConnection();
	}
	public class ConnectionServiceAsync : IConnectionServiceAsync
	{
		public string GetConnection()
		{
			return "Server=SKINOFME;Database=EmpresaDB;Trusted_Connection=True;TrustServerCertificate=True";
		}




		public async Task<DataSet> ExecuteSelectAsync(string query)
		{
			DataSet ds = new DataSet();

			try
			{

				using var cnn = new SqlConnection(GetConnection());

				await cnn.OpenAsync();
				SqlDataAdapter adapter = new SqlDataAdapter(query, cnn);

				adapter.Fill(ds);

			}
			catch (Exception ex) 
			{
				Console.WriteLine("Error al ejecutar el Select. error: " + ex.ToString());
			}

			return ds;
        }

		public async Task ExecuteCommand(string query, SqlParameter[] parameters)
		{




		}




	}
}
