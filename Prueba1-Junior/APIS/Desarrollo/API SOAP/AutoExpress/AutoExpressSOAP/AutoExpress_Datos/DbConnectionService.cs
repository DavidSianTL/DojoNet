using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AutoExpress_Datos
{
	public interface IDbConnectionService
	{
		DataSet ExecuteStoredProcedure(string sp);
		DataSet ExecuteStoredProcedure(string sp, List<SqlParameter> parameters);
		bool ExecuteStoredProcedureNonQuery(string sp, List<SqlParameter> parameters = null);
	}

	public class DbConnectionService : IDbConnectionService
	{
		
		private readonly string _connectionString = "Server=localhost,1433;Database=AutoExpressDB;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout = 5;";

		// Metodo para ejecutar un stored procedure que retorna datos pero no necesita parametros      
		public DataSet ExecuteStoredProcedure(string sp)
		{
			var ds = new DataSet();

			try
			{

				using (SqlConnection cnn = new SqlConnection(_connectionString))
				{ 
					cnn.Open();
					SqlDataAdapter adapter = new SqlDataAdapter(sp, cnn);

					adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
					adapter.Fill(ds);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex + " ------ " + ex.Message);
				throw; // para que se vea en navegador
			}



			return ds;
		}

		// Metodo para ejecutar un stored procedure que retorna datos y necesita parametros
		public DataSet ExecuteStoredProcedure(string sp, List<SqlParameter> parameters)
		{
			var ds = new DataSet();
			try
			{
				using (SqlConnection cnn = new SqlConnection(_connectionString))
				{


					cnn.Open();

					// Se crea un SqlDataAdapter para ejecutar el stored procedure
					SqlDataAdapter adapter = new SqlDataAdapter(sp, cnn);
					adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
					if (parameters != null && parameters.Count > 0)
					{
						adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
					}
					adapter.Fill(ds);

				}

			}
			catch (Exception ex)
			{
				
			}

			return ds;
		}

		// Metodo para ejecutar un stored procedure que no retorna datos y necesita parametros
		public bool ExecuteStoredProcedureNonQuery(string sp, List<SqlParameter> parameters = null)
		{
			try
			{
				using (SqlConnection cnn = new SqlConnection(_connectionString))
				{


					cnn.Open();
					using (SqlCommand cmd = new SqlCommand(sp, cnn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						if (parameters != null && parameters.Count > 0)
						{
							cmd.Parameters.AddRange(parameters.ToArray()); 
						}
						cmd.ExecuteNonQuery(); 
					}
				}

				return true;
			}
			catch (Exception ex)
            {
                Console.WriteLine(ex + " ------------" + ex.Message); 
				
                return false;
			}
		}



	}
}
